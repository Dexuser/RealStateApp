using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Dtos.Property;

public class PropertyDto
{
    public required int Id { get; set; }
    public required string Code { get; set; } // 6 digit unique

    public required int PropertyTypeId { get; set; }
    public PropertyTypeDto? PropertyType { get; set; }

    public required int SaleTypeId { get; set; }
    public SaleTypeDto? SaleType { get; set; }

    public required decimal Price { get; set; }
    public required double SizeInMeters { get; set; }
    public required int Rooms { get; set; }
    public required int Bathrooms { get; set; }
    public required string Description { get; set; }
    public bool IsAvailable { get; set; } // true = available, false = sold

    public required string AgentId { get; set; }

    // Navigation
    public ICollection<PropertyImageDto> PropertyImages { get; set; } = [];
    public ICollection<PropertyImprovementDto> PropertyImprovements { get; set; } = [];
    public ICollection<OfferDto> Offers { get; set; } = [];
    public ICollection<ChatMessageDto> ChatMessages { get; set; } = [];
    public ICollection<FavoritePropertyDto> FavoriteProperties { get; set; } = [];
    public UserDto? Agent { get; set; }
}
