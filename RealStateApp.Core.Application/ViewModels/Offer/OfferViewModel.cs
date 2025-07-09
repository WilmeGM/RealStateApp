namespace RealStateApp.Core.Application.ViewModels.Offer
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string OfferStatus { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
