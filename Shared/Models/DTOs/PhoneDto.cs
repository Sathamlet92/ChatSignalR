using System.ComponentModel.DataAnnotations;

namespace BlazingChat.Shared.Models.DTOs;
public class PhoneDto
{
    public string AreaCode { get; set; } = null!;
    [Phone]
    public string Phone {get; set;} = null!;
}