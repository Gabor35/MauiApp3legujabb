using Firebase.Database;
using Firebase.Database.Query;
using MauiApp3.Models;

namespace MauiApp3.Services
{
    public class FirebaseChatService
    {
        private readonly FirebaseClient _firebase;

        public FirebaseChatService()
        {
            _firebase = new FirebaseClient("https://esemenyrendezo-71f5b-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            await _firebase
                .Child("chats")
                .PostAsync(new
                {
                    message = message.Message,
                    timestamp = message.Timestamp,
                    userId = message.UserId
                });
        }

        public async Task<List<ChatMessage>> GetMessagesAsync()
        {
            var messages = await _firebase
                .Child("chats")
                .OrderBy("timestamp")
                .OnceAsync<ChatMessage>();

            return messages.Select(m => new ChatMessage
            {
                Message = m.Object.Message,
                Timestamp = m.Object.Timestamp,
                UserId = m.Object.UserId
            }).OrderBy(m => m.Timestamp).ToList();
        }
    }
}