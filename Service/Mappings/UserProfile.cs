
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
            .ForMember(dest => 
                dest.SecondLastName, opt => opt.MapFrom(src => 
                src.LastName!.Split().Count() > 1 ? src.LastName!.Split()[1] : null));
        CreateMap<Email, UserDto>().ForMember( dest => dest.EmailAddress,opt => opt.MapFrom(src => src.EmailAddress));
        CreateMap<Phone, UserDto>().ForMember( dest => dest.Phone,opt => opt.MapFrom(src => src.Tel));
                
        CreateMap<UserDto, User>()
            .ForMember(dest => 
                dest.LastName, opt => opt.MapFrom(src => 
                $"{src.LastName} {src.SecondLastName ?? string.Empty}"));
        CreateMap<UserDto, Email>().ForMember( dest => dest.EmailAddress,opt => opt.MapFrom(src => src.EmailAddress));
        CreateMap<UserDto, Phone>().ForMember( dest => dest.Tel,opt => opt.MapFrom(src => src.Phone));

        CreateMap<ProfileVM, UserDto>();
        CreateMap<IProfileVM, UserDto>();
        CreateMap<UserDto, ProfileVM>(); 
        CreateMap<UserDto, IProfileVM>().ConvertUsing(parent => new ProfileVM());
    }
}
