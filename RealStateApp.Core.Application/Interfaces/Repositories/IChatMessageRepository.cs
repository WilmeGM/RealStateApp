using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
        Task<List<ChatMessage>> GetMessagesByPropertyAndUsersAsync(int propertyId,
                                                                    string senderId,
                                                                    string receiverId);
        Task<List<ChatMessage>> GetMessagesByUsersAsync(string senderId, string receiverId);

    }
}
