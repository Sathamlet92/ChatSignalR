
using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Domain.Models.Entites;

namespace BlazingChat.Service.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        #region User
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Logins.First(l => l.UserId.Equals(src.UserId)).UserName))
            .ForMember(dest => 
                dest.LastName, opt => opt.MapFrom(src => 
                src.LastName!.Split().Count() > 1 ? src.LastName.Split()[0] : src.LastName.Split()[1]))
            .ForMember(dest => 
                dest.SecondLastName, opt => opt.MapFrom(src => 
                src.LastName!.Split().Count() > 1 ? src.LastName!.Split()[1] : null))
            .ForMember(dest => 
                dest.Emails, opt => opt.MapFrom(src => 
                src.Emails))
            .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones))
            .ForMember(dest => dest.UrlImageProfile, opt => opt.MapFrom(dest => dest.ProfilePictureUrl));

        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Phones, opt => opt.Ignore())
            .ForMember(dest => dest.Emails, opt => opt.Ignore())
            .ForMember(dest => 
                dest.LastName, opt => opt.MapFrom(src => 
                $"{src.LastName} {src.SecondLastName ?? string.Empty}".TrimEnd()))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.UrlImageProfile));               
        #endregion

        #region  Telephone
        CreateMap<Phone, PhoneDto>()
            .ForMember( dest => 
                dest.Phone, opt => opt.MapFrom(src => 
                src.Tel))
            .ForMember( dest => 
                dest.AreaCode,opt => opt.MapFrom(src => 
                src.AreaCode));
        CreateMap<PhoneDto, Phone>()
            .ForMember( dest => 
                dest.Tel, opt => opt.MapFrom(src => 
                src.Phone))
            .ForMember( dest => 
                dest.AreaCode, opt => opt.MapFrom(src => 
                src.AreaCode))
            .ForMember(dest => dest.PhoneId, opt => opt.MapFrom(src => src.PhoneId));;
        #endregion
        
        #region Email
        CreateMap<EmailDto, Email>()
            .ForMember( dest => 
                dest.EmailAddress,opt => opt.MapFrom(src => 
                src.EmailAddress))
            .ForMember(dest => dest.EmailId, opt => opt.MapFrom(src => src.EmailId));
        CreateMap<Email, EmailDto>()
            .ForMember( dest => 
                dest.EmailAddress,opt => opt.MapFrom(src => 
                src.EmailAddress));
        #endregion

        CreateMap<ProfileVM, UserDto>();
        CreateMap<IProfileVM, UserDto>().ForMember(dest => dest.UrlImageProfile, opt => opt.MapFrom(dest => dest.UrlImageProfile));
        CreateMap<UserDto, ProfileVM>(); 
        CreateMap<UserDto, IProfileVM>().ConstructUsing(parent => new ProfileVM());
        CreateMap<ILoginVM, LoginDto>();
        CreateMap<UserDto, IMessagingProfileVM>()   
            .ConstructUsing(parent => new MessagingProfileVM())
            .ForMember(dest => 
                dest.UrlPicture, opt => opt.MapFrom(src => 
                src.UrlImageProfile))
            .ForMember(dest => 
                dest.Email, opt => opt.MapFrom(src => 
                src.Emails.First().EmailAddress))
            .ForMember(dest => 
                dest.Nombre, opt => 
                opt.MapFrom(src => $"{src.FirstName} {src.SecondName ?? ""} {src.LastName}"))
            .ForMember(dest => 
                dest.UserName, opt => opt.MapFrom(src => 
                src.UserName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Convert.ToString(src.UserId)));      
    }
}
