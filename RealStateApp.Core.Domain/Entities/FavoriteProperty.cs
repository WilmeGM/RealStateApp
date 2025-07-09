using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class FavoriteProperty : AuditableBaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string UserId { get; set; }
    }
}