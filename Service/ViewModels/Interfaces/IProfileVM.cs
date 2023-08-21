using System.ComponentModel.DataAnnotations;

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
    public string EmailAddress { get; set; }
    [Required]
    public string Phone { get; set; }
    public string? Message { get; set; }
    public List<AreaCodeVM>? AreaCodes { get; set; }

    public Task UpdateProfile(IProfileVM model);
    public Task GetProfile(long userId);
    public IAsyncEnumerable<string> GetAreaCodes();
}