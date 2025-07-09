using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class ChatMessageRepository(ApplicationDbContext dbContext) : GenericRepository<ChatMessage>(dbContext), IChatMessageRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<ChatMessage>> GetMessagesByPropertyAndUsersAsync(int propertyId, string senderId, string receiverId)
        {
            return await _dbContext.ChatMessages
                .Where(cm => cm.PropertyId == propertyId ||
                             ((cm.SenderId == senderId && cm.ReceiverId == receiverId) ||
                              (cm.SenderId == receiverId && cm.ReceiverId == senderId)))
                .OrderBy(cm => cm.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesByUsersAsync(string senderId, string receiverId)
        {
            return await _dbContext.ChatMessages
                .Where(cm =>
                    (cm.SenderId == senderId && cm.ReceiverId == receiverId) ||
                    (cm.SenderId == receiverId && cm.ReceiverId == senderId))
                .OrderBy(cm => cm.CreatedAt)
                .ToListAsync();
        }
    }
}
