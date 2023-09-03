using System.Net.Http.Json;
using AutoMapper;
using BlazingChat.Shared.Models.DTOs;

namespace BlazingChat.Service.ViewsModels;
public class MessagingProfileVM : IMessagingProfileVM
{
    public string UserId { get; set; } = null!;
    public string? UrlPicture { get; set; }
    public string? Nombre { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }

    private readonly HttpClient? _client;
    private readonly IMapper? _mapper;

    public MessagingProfileVM()
    {
        
    }
    public MessagingProfileVM(HttpClient? client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task GetProfile(long idUser)
    {
        var response = await _client!.GetFromJsonAsync<UserDto>($"api/User/getprofile/{idUser}");
        _mapper!.Map(response, this);
    }
}