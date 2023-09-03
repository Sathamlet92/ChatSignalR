using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Domain.Models.Entites;

namespace BlazingChat.Service.Mappings;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactVM, ContactDto>()
            .ForMember(dto => dto.FirstName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 1 ? vm.FirstName.Split()[0] : vm.FirstName))
            .ForMember(dto => dto.SecondName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 1 ? vm.FirstName.Split()[1] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 1 ? vm.LastName.Split()[0] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 1 ? vm.LastName.Split()[1] : null));

        CreateMap<IContactVM, ContactDto>()
            .ForMember(dto => dto.FirstName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 1 ? vm.FirstName.Split()[0] : vm.FirstName))
            .ForMember(dto => dto.SecondName, 
                       opt => opt.MapFrom(vm => vm.FirstName.Split().Length > 1 ? vm.FirstName.Split()[1] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 1 ? vm.LastName.Split()[0] : null))
            .ForMember(dto => dto.LastName, 
                       opt => opt.MapFrom(vm => vm.LastName.Split().Length > 1 ? vm.LastName.Split()[1] : null));

        CreateMap<ContactDto, ContactVM>()
            .ForMember(vm => vm.FirstName, 
                      opt => opt.MapFrom(dto => $"{dto.FirstName} {dto.SecondName}"))
            .ForMember(vm => vm.LastName, 
                      opt => opt.MapFrom(dto => $"{dto.LastName} {dto.LastSecondName}"));
        CreateMap<ContactDto, IContactVM>().ConstructUsing(parent => new ContactVM())
            .ForMember(vm => vm.Email, 
                       opt => opt.MapFrom(dto => dto.Emails.Single(e => e.HasPrincipal!.Value).EmailAddress))
            .ForMember(vm => vm.Phone, 
                       opt => opt.MapFrom(dto => $" {dto.Phones.First().AreaCode}-{dto.Phones.First().Phone}"))
            .ForMember(vm => vm.FirstName, 
                       opt => opt.MapFrom(dto => $"{dto.FirstName} {dto.SecondName}"))
            .ForMember(vm => vm.LastName, 
                       opt => opt.MapFrom(dto => $"{dto.LastName} {dto.LastSecondName}"))
            .ForMember(vm => vm.UrlImage, 
                        opt => opt.MapFrom(src => src.UrlImage))
            .ForMember(vm => vm.UserId, 
                        opt => opt.MapFrom(src => src.UserId));
        
        CreateMap<Contact, ContactDto>().
            ForMember(dest => dest.Phones, 
                    opt => opt.MapFrom(src => src.ContactUser.Phones)).
            ForMember(dest => dest.Emails, 
                    opt => opt.MapFrom(src => src.ContactUser.Emails)).
            ForMember(dest => dest.FirstName, 
                       opt => opt.MapFrom(src => src.ContactName.Split().Length > 1 ? src.ContactName.Split()[0] : src.ContactName)).
            ForMember(dest => dest.LastName, 
                       opt => opt.MapFrom(src => src.ContactLastName!.Split().Length > 1 ? src.ContactLastName.Split()[0] : src.ContactLastName)).
            ForMember(dest => dest.SecondName, 
                       opt => opt.MapFrom(src => src.ContactName!.Split().Length > 1 ? src.ContactName.Split()[1] : null)).
            ForMember(dest => dest.LastSecondName, 
                       opt => opt.MapFrom(src => src.ContactLastName!.Split().Length > 1 ? src.ContactLastName.Split()[1] : null)).
            ForMember(dest => dest.UrlImage, 
                       opt => opt.MapFrom(src => src.ContactUser.ProfilePictureUrl))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ContactUser.Logins.First().UserName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ContactUser.UserId));
        
        CreateMap<IContactVM, MessagingContactVM>()
            .ForMember(dest => 
                dest.Name, opt => opt.MapFrom(
                src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => 
                dest.Email, opt => opt.MapFrom(
                src => src.Email))
            .ForMember(dest => 
                dest.UrlPicture, opt => opt.MapFrom(
                src => src.UrlImage))
            .ForMember(dest => 
                dest.ContactId, opt => opt.MapFrom(
                src => Convert.ToString(src.ContactId)))
            .ForMember(dest => 
                dest.UserId, opt => opt.MapFrom(
                src => Convert.ToString(src.UserId)))
            .ForMember(dest => 
                dest.UserName, opt => opt.MapFrom(
                src => src.UserName));
            
        CreateMap<Email, EmailDto>();
        CreateMap<Phone, PhoneDto>()
            .ForMember(dest => dest.Phone, 
                opt => opt.MapFrom(src => src.Tel))
            .ForMember(dest => dest.AreaCode, opt => 
                opt.MapFrom(src => src.AreaCode));
    }    
}