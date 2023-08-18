
using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Service.ViewsModels;

namespace BlazingChat.Service.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ProfileVM, UserDto>();
        CreateMap<IProfileVM, UserDto>();
        CreateMap<UserDto, ProfileVM>(); 
        CreateMap<UserDto, IProfileVM>().ConvertUsing(parent => new ProfileVM());
    }
}
