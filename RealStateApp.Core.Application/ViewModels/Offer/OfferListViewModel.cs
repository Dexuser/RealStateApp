namespace RealStateApp.Core.Application.ViewModels.Offer;

public class OfferListViewModel
{
    public int PropertyId { get; set; }
    
    public string ClientId { get; set; }
    public bool CanCreateOffer { get; set; }
    public List<OfferViewModel> Offers { get; set; } = new();
}
