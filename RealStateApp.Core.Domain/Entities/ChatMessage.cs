using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class ChatMessage : AuditableBaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int? PropertyId { get; set; }
        public Property? Property { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
