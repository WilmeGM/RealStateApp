using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Offer : AuditableBaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string OfferStatus { get; set; }
    }
}
