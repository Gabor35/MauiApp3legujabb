using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MauiApp3.Models;
using MauiApp3.DTOs;
using MauiApp3.Services;

namespace MauiApp3.Services
{
    public class AuthService
    {
        public LoggedUser? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser is not null && !string.IsNullOrEmpty(CurrentUser.Token);

        public async Task InitializeAsync(ApiService apiService)
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                apiService.SetAuthToken(token);
                CurrentUser = await apiService.GetStoredUserAsync();
            }
        }

        public async Task SetUserAsync(LoggedUser user, ApiService apiService)
        {
            CurrentUser = user;
            apiService.SetAuthToken(user.Token);
            await SecureStorage.SetAsync("auth_token", user.Token);
        }

        public async Task LogoutAsync(ApiService apiService)
{
    var token = await SecureStorage.GetAsync("auth_token");
    if (!string.IsNullOrEmpty(token))
    {
        await apiService.LogoutAsync(); 
        await SecureStorage.SetAsync("auth_token", string.Empty);
    }
    CurrentUser = null;
}

    }
}
