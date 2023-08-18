using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Service.ViewsModels;

namespace BlazingChat.Service.Mappings;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactVM, ContactDto>()
            .ForMember(dto => dto.FirstName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 0 ? vm.FirstName.Split()[0] : vm.FirstName))
            .ForMember(dto => dto.SecondName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 0 ? vm.FirstName.Split()[1] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 0 ? vm.LastName.Split()[0] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 0 ? vm.LastName.Split()[1] : null));

        CreateMap<IContactVM, ContactDto>()
            .ForMember(dto => dto.FirstName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 0 ? vm.FirstName.Split()[0] : vm.FirstName))
            .ForMember(dto => dto.SecondName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 0 ? vm.FirstName.Split()[1] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 0 ? vm.LastName.Split()[0] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 0 ? vm.LastName.Split()[1] : null));

        CreateMap<ContactDto, ContactVM>()
            .ForMember(vm => vm.FirstName, 
                      opt => opt.MapFrom(dto => $"{dto.FirstName} {dto.SecondName}"))
            .ForMember(vm => vm.LastName, 
                      opt => opt.MapFrom(dto => $"{dto.LastName} {dto.LastSecondName}"));
        CreateMap<ContactDto, IContactVM>().ConstructUsing(parent => new ContactVM())
            .ForMember(vm => vm.FirstName, 
                       opt => opt.MapFrom(dto => $"{dto.FirstName} {dto.SecondName}"))
            .ForMember(vm => vm.LastName, 
                       opt => opt.MapFrom(dto => $"{dto.LastName} {dto.LastSecondName}"));
    }    
}