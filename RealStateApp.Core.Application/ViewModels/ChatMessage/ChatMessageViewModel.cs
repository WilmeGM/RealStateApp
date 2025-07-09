namespace RealStateApp.Core.Application.ViewModels.ChatMessage
{
    public class ChatMessageViewModel
    {
        public string SenderId { get; set; }
        public string Message { get; set; }
        public bool IsSender { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
