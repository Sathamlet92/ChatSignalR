using AutoMapper;
using BlazingChat.Domain.Models.Entites;
using BlazingChat.Shared.Models.DTOs;

namespace BlazingChat.Service.Mappings;

public class SettingsProfile : Profile
{
    public SettingsProfile()
    {
        CreateMap<SettingsDto, ISettingsVM>().ConstructUsing(s => new SettingsVM());
        CreateMap<User, SettingsDto>();
        CreateMap<SettingsDto, User>();   
    }
}
