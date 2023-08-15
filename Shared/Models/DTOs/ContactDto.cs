using System.ComponentModel.DataAnnotations;

namespace BlazingChat.Shared.Models.DTOs;

public class ContactDto
{
    public long ContactId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? SecondName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? LastSecondName { get; set; } 
    [Phone]
    public string Phone { get; set; } = string.Empty;
    public string? AreaCode { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }

    public string LastMessage { get; set; } = string.Empty;
    public string NumNoReadMessages {get; set;} = string.Empty;
    public string ActiveClass = string.Empty;

    public ContactDto()
    {
        
    }

    public ContactDto(int contactId, string firstName, string lastName)
    {
        ContactId = contactId;
        FirstName = firstName;
        LastName = lastName;
    }
}
