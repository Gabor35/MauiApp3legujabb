using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp3.Models;
using MauiApp3.DTOs;

namespace MauiApp3.Services
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }

    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://esemenyrendezo1.azurewebsites.net/api/");
            //_httpClient.BaseAddress = new Uri("http://localhost:5000/api");
        }

        public string BaseImageUrl { get; } = "http://files.esemenyrendezo.nhely.hu/Images/";

        public void SetAuthToken(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Esemeny>> GetEsemenyekAsync()
        {
            var response = await _httpClient.GetAsync("esemeny");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Esemeny>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Esemeny>();
        }

        public async Task<string?> GetSaltAsync(string felhasznaloNev)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Username = felhasznaloNev }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"Login/GetSalt/{felhasznaloNev}", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }


        public async Task<LoggedUser?> LoginAsync(string username, string password)
        {
            // DTO létrehozása
            LoginDTO dto = new LoginDTO
            {
                LoginName = username,
                TmpHash = password
            };

            // JSON tartalom előállítása
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Ellenőrzés: Mit küldünk el?
            Console.WriteLine($"Küldött JSON: {json}");

            // POST kérés a szerverhez
            var response = _httpClient.PostAsync("Login", content).Result;

            // Ellenőrizzük a választ
            var responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Szerver válasza: {response.StatusCode} - {responseText}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // JSON válasz deszerializálása
            return JsonSerializer.Deserialize<LoggedUser>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<bool> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("Login/Logout", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<LoggedUser?> GetStoredUserAsync()
        {
            var response = await _httpClient.GetAsync("Login/LoggedUser");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoggedUser>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> SaveEsemenyAsync(Esemeny esemeny)
        {
            var content = new StringContent(JsonSerializer.Serialize(esemeny), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Esemeny", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException($"Hiba történt az esemény mentésekor: {response.StatusCode} - {error}");
            }
            return true;
        }

        public async Task<List<Chat>> GetChatMessagesAsync()
        {
            var response = await _httpClient.GetAsync("ChatMessage");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Chat>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Chat>();
        }

        public async Task<bool> SaveEsemenyAsync(Esemeny esemeny, string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ApiException("Nincs érvényes token a kéréshez.");
            }

            var content = new StringContent(JsonSerializer.Serialize(esemeny), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "Esemeny")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException($"Hiba történt az esemény mentésekor: {response.StatusCode} - {error}");
            }

            return true;
        }

        public async Task<bool> LogoutAsync(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var logoutData = new { Token = token };
            var content = new StringContent(JsonSerializer.Serialize(logoutData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Login/Logout", content);
            return response.IsSuccessStatusCode;
        }

        public string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}