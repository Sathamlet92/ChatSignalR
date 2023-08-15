using AutoMapper;
using BlazingChat.Client.ViewsModels;
using BlazingChat.Shared.Models.DTOs;

namespace BlazingChat.Client;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ProfileView, UserDto>();
        CreateMap<UserDto, ProfileView>();    
    }
}
