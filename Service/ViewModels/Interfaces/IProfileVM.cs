using System.ComponentModel.DataAnnotations;
using BlazingChat.Shared.Models.DTOs;

namespace BlazingChat.Service.ViewsModels;
public interface IProfileVM
{    
    public long UserId { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string? SecondName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string? SecondLastName { get; set; }
    [Required]
    public List<EmailDto> Emails { get; set; }
    [Required]
    public List<PhoneDto> Phones { get; set; }
    public string? Message { get; set; }
    public List<AreaCodeVM>? AreaCodes { get; set; }
    public string? UrlImageProfile { get; set; }
    public Task UpdateProfile();
    public Task GetProfile(long userId);
    public IAsyncEnumerable<string> GetAreaCodes();
}