using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.ChatMessage;
using RealStateApp.Core.Application.ViewModels.FavoriteProperty;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Application.ViewModels.PropertyImage;
using RealStateApp.Core.Application.ViewModels.PropertyImprovement;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.Property;

public class PropertyViewModel
{
    public required int Id { get; set; }
    public required string Code { get; set; } // 6 digit unique

    public required int PropertyTypeId { get; set; }
    public PropertyTypeViewModel? PropertyType { get; set; }

    public required int SaleTypeId { get; set; }
    public SaleTypeViewModel? SaleType { get; set; }

    public required decimal Price { get; set; }
    public required double SizeInMeters { get; set; }
    public required int Rooms { get; set; }
    public required int Bathrooms { get; set; }
    public required string Description { get; set; }
    public bool IsAvailable { get; set; } // true = available, false = sold

    public required string AgentId { get; set; }

    // Navigation
    public ICollection<PropertyImageViewModel> PropertyImages { get; set; } = [];
    public ICollection<PropertyImprovementViewModel> PropertyImprovements { get; set; } = [];
    public ICollection<OfferViewModel> Offers { get; set; } = [];
    public ICollection<ChatMessageViewModel> ChatMessages { get; set; } = [];
    public ICollection<FavoritePropertyViewModel> FavoriteProperties { get; set; } = [];
    public UserViewModel? Agent { get; set; }
}
