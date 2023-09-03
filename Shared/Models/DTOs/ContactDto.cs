using System.ComponentModel.DataAnnotations;

namespace BlazingChat.Shared.Models.DTOs;

public class ContactDto
{
    public long ContactId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? SecondName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? LastSecondName { get; set; }
    public string? UrlImage { get; set; }
    public string UserName { get; set; }  = string.Empty;
    public List<PhoneDto> Phones { get; set; }
    public List<EmailDto> Emails { get; set; }

    public string LastMessage { get; set; } = string.Empty;
    public string NumNoReadMessages {get; set;} = string.Empty;
    public string ActiveClass = string.Empty;

    public ContactDto()
    {
        Phones = new();
        Emails = new();    
    }

    public ContactDto(int contactId, string firstName, string lastName) : this()
    {
        ContactId = contactId;
        FirstName = firstName;
        LastName = lastName;
    }
}
