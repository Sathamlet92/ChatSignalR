using System.ComponentModel.DataAnnotations;

namespace BlazingChat.Shared.Models.DTOs;
public class EmailDto
{
    [Required, EmailAddress]
    public string EmailAddress { get; set; } = null!;
    public bool? HasPrincipal { get; set; } 
}