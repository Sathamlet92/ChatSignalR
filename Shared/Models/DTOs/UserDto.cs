namespace BlazingChat.Shared.Models.DTOs;

public class UserDto
{
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? SecondName {get; set;}
    public string LastName { get; set; } = string.Empty;
    public string? SecondLastName { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string?  AboutMe { get; set; }
}
