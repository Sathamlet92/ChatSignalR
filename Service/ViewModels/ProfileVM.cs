using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using AutoMapper;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Shared.Models.Reponse;
using Newtonsoft.Json;

namespace BlazingChat.Service.ViewsModels;

public class ProfileVM : IProfileVM
{
    public long UserId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; } = string.Empty;
    public string? SecondName { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; } = string.Empty;
    public string? SecondLastName { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string EmailAddress { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string Phone { get; set; } = string.Empty;
    public string? Message { get; set; } = string.Empty;
    private readonly IMapper? _mapper;
    private HttpClient? _client;

    public ProfileVM()
    {
        
    }

    public ProfileVM(IMapper mapper, HttpClient client)
    {
        _mapper = mapper;
        _client = client;
    }

    public async Task GetProfile()
    {
        var response = await _client!.GetFromJsonAsync<UserDto>("api/User/getprofile/1");
        _mapper!.Map(response, this);
    }

    public async Task UpdateProfile(IProfileVM model)
    {
        var entReq = _mapper!.Map<UserDto>(model);
        var result = await _client!.PutAsJsonAsync<UserDto>("api/user", entReq);
        var response = await result.Content.ReadAsStringAsync();
        var objRes = JsonConvert.DeserializeObject<ResponseOut<UserDto>>(response);
        model = _mapper.Map(objRes!.Data, model);
    }
}