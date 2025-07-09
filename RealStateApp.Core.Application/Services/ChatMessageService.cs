using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class ChatMessageService(IChatMessageRepository chatMessageRepository) : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository = chatMessageRepository;

        public async Task<List<ChatMessage>> GetMessagesAsync(int propertyId, string senderId, string receiverId)
            => await _chatMessageRepository.GetMessagesByPropertyAndUsersAsync(propertyId, senderId, receiverId);

        public async Task<List<ChatMessage>> GetMessagesBetweenUsersAsync(string senderId, string receiverId)
            => await _chatMessageRepository.GetMessagesByUsersAsync(senderId, receiverId);

        public async Task<ChatMessage> SendMessageAsync(ChatMessage message)
            => await _chatMessageRepository.AddAsync(message);

        public async Task<List<string>> GetClientsForAgentAsync(string agentId)
        {
            var messages = await _chatMessageRepository.GetAllAsync();
            return messages
                .Where(m => m.ReceiverId == agentId)
                .Select(m => m.SenderId)
                .Distinct()
                .ToList();
        }
    }
}
