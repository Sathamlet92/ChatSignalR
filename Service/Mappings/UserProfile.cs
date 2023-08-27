
using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Domain.Models.Entites;

namespace BlazingChat.Service.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Logins.First(l => l.UserId.Equals(src.UserId)).UserName))
            .ForMember(dest => 
                dest.LastName, opt => opt.MapFrom(src => 
                src.LastName!.Split().Count() > 1 ? src.LastName.Split()[0] : src.LastName.Split()[1]))
            .ForMember(dest => 
                dest.SecondLastName, opt => opt.MapFrom(src => 
                src.LastName!.Split().Count() > 1 ? src.LastName!.Split()[1] : null))
            .ForMember(dest => 
                dest.EmailAddress, opt => opt.MapFrom(src => 
                src.Emails.First(e => e.HasPrincipal!.Value).EmailAddress))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => $"{src.Phones.First().AreaCode}-{src.Phones.First().Tel}"));
                
        CreateMap<Email, UserDto>().ForMember( dest => dest.EmailAddress,opt => opt.MapFrom(src => src.EmailAddress));
        CreateMap<Phone, UserDto>().ForMember( dest => dest.Phone,opt => opt.MapFrom(src => src.Tel));
                
        CreateMap<UserDto, User>()
            .ForMember(dest => 
                dest.LastName, opt => opt.MapFrom(src => 
                $"{src.LastName} {src.SecondLastName ?? string.Empty}".TrimEnd()));
        CreateMap<UserDto, Email>().ForMember( dest => dest.EmailAddress,opt => opt.MapFrom(src => src.EmailAddress));
        CreateMap<UserDto, Phone>()
            .ForMember(dest => dest.Tel, opt => opt.MapFrom(src => src.Phone.Split()[1]))
            .ForMember(dest => dest.AreaCode, opt => opt.MapFrom(src => src.Phone.Split()[0]));

        CreateMap<ProfileVM, UserDto>();
        CreateMap<IProfileVM, UserDto>();
        CreateMap<UserDto, ProfileVM>(); 
        CreateMap<UserDto, IProfileVM>().ConstructUsing(parent => new ProfileVM());
        CreateMap<ILoginVM, LoginDto>();
    }
}
