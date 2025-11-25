using AutoMapper;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Mappings;

public class UserDtoMappingProfile : Profile
{
    public UserDtoMappingProfile()
    {
        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.IdentityCardNumber, opt => opt.MapFrom(src => src.IdentityCardNumber))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.ProfileImagePath))
            .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.RegisteredAt))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.EmailConfirmed))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.IdentityCardNumber, opt => opt.MapFrom(src => src.IdentityCardNumber))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.ProfileImagePath))
            .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.RegisteredAt))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<UserSaveDto, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.IdentityCardNumber, opt => opt.MapFrom(src => src.IdentityCardNumber))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.ProfileImagePath))
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.IdentityCardNumber, opt => opt.MapFrom(src => src.IdentityCardNumber))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.ProfileImagePath))

            ;

    }
}