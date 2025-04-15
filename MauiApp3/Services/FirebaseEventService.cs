using MauiApp3.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using System.IO;

namespace MauiApp3.Services
{
    public class FirebaseEventService
    {
        private readonly HttpClient _httpClient;
        private const string FirestoreUrl = "https://firestore.googleapis.com/v1/projects/esemenyrendezo-71f5b/databases/(default)/documents/events";
        private readonly string _bucketName = "esemenyrendezo-71f5b.appspot.com";
        private readonly StorageClient _storageClient;

        public FirebaseEventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _storageClient = StorageClient.Create();
        }

        public async Task<string> UploadImageToStorage(byte[] imageBytes, string fileName)
        {
            var filePath = $"events-images/{fileName}";
            using (var stream = new MemoryStream(imageBytes))
            {
                var storageObject = await _storageClient.UploadObjectAsync(_bucketName, filePath, "image/jpeg", stream);
                return storageObject.MediaLink;
            }
        }

        public async Task AddEventToFirestore(EventModel newEvent)
        {
            var eventJson = new
            {
                fields = new
                {
                    Cime = new { stringValue = newEvent.Cime },
                    Helyszin = new { stringValue = newEvent.Helyszin },
                    Datum = new { stringValue = newEvent.Datum.ToString("yyyy-MM-dd'T'HH:mm:ss") },
                    Leiras = new { stringValue = newEvent.Leiras },
                    Kepurl = new { stringValue = newEvent.Kepurl }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(eventJson), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(FirestoreUrl, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
