using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessage>> GetMessagesAsync(int propertyId, string senderId, string receiverId);
        Task<List<ChatMessage>> GetMessagesBetweenUsersAsync(string senderId, string receiverId);
        Task<ChatMessage> SendMessageAsync(ChatMessage message);
        Task<List<string>> GetClientsForAgentAsync(string agentId);
    }
}
