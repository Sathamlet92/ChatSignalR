using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Shared.Models.Reponse;
using Newtonsoft.Json;

namespace BlazingChat.Service.ViewsModels;

public class LoginVM : ILoginVM
{
    private readonly HttpClient? _client;
    private readonly IMapper? _mapper;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Message { get; set; }

    public LoginVM()
    {
        
    }
    public LoginVM(HttpClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    

    public async Task<bool> LoginUser()
    {
        var request = _mapper!.Map(this, new LoginDto());
        var json = JsonConvert.SerializeObject(request);
        var buffer = Encoding.UTF8.GetBytes(json);
        var byteArr = new ByteArrayContent(buffer);
        byteArr.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await _client!.PostAsync("api/user/login", byteArr);
        var body = await response.Content.ReadAsStringAsync();
        var responseOb = JsonConvert.DeserializeObject<ResponseOut<LoginDto>>(body);
        Message = responseOb!.Message;
        return responseOb.Success;
    }
}
