namespace BlazingChat.Shared.Models.DTOs;

public class UserDto
{
    public long UserId { get; set; }
    public string? UserName {get; set;}
    public string FirstName { get; set; } = string.Empty;
    public string? SecondName {get; set;}
    public string LastName { get; set; } = string.Empty;
    public string? SecondLastName { get; set; }
    public List<EmailDto> Emails { get; set; } = null!;
    public List<PhoneDto> Phones { get; set; } = null!;
    public string? Message { get; set; }
    public string?  AboutMe { get; set; }
    public string? UrlImageProfile { get; set; }

    public UserDto()
    {
        Emails = new();
        Phones = new();    
    }
}
