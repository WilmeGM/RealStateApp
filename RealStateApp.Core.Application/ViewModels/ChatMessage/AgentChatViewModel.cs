namespace RealStateApp.Core.Application.ViewModels.ChatMessage
{
    public class AgentChatViewModel
    {
        public string ClientId { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; } = new();
    }
}
