using BlazingChat.Shared;

namespace BlazingChat.Service;

public interface ILoginService
{   
    public string User { get; set; }
    public string Password { get; set; }
    public Task<bool> LoginUser(ILoginService login);
}
