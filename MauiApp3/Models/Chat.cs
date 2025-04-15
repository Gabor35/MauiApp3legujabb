namespace MauiApp3.Models
{
    public class ChatMessage
    {
        public string Message { get; set; } = string.Empty;
        public long Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
