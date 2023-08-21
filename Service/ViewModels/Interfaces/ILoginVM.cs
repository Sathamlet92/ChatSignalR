namespace BlazingChat.Service.ViewsModels;

public interface ILoginVM
{
    public string User { get; set; }
    public string Password { get; set; }
    public string? Message { get; set; }
    public Task<bool> LoginUser();
}
