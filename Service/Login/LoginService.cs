namespace BlazingChat.Service;

using System.Net.Http.Json;
using BlazingChat.Shared;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Shared.Models.Reponse;
using Newtonsoft.Json;

public class LoginService : ILoginService
{
    private readonly IHttpClientFactory _factoryClient;

    public LoginService(IHttpClientFactory factoryClient)
    {
        _factoryClient = factoryClient;
    }

    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public async Task<bool> LoginUser(ILoginService login)
    {
        using var client = _factoryClient.CreateClient("BlazingChatClient");
        var result = await client.PostAsJsonAsync<ILoginService>("user/loginuser", this);
        var response = await result.Content.ReadAsStringAsync();
        var objRes =  JsonConvert.DeserializeObject<ResponseOut<UserDto>>(response);
        return objRes!.Success;
    }
}
