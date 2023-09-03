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
    public List<EmailDto> Emails { get; set; } = new();
    [Required(AllowEmptyStrings = false)]
    public List<PhoneDto> Phones { get; set; } = new();
    public string? Message { get; set; } = string.Empty;
    public List<AreaCodeVM>? AreaCodes { get; set; }
    
    public string? UrlImageProfile { get; set; }

    private readonly IMapper? _mapper;
    private HttpClient? _client;

    public ProfileVM()
    {
        AreaCodes = new();        
    }

    public ProfileVM(IMapper mapper, HttpClient client) :this()
    {
        _mapper = mapper;
        _client = client;
    }

    public async Task GetProfile(long userId)
    {
        var response = await _client!.GetFromJsonAsync<UserDto>($"api/User/getprofile/{userId}");
        var algo = _mapper!.Map(response, this);
    }

    public async Task UpdateProfile()
    {
        var entReq = _mapper!.Map<UserDto>(this);
        var result = await _client!.PutAsJsonAsync<UserDto>("api/user", entReq);
        var response = await result.Content.ReadAsStringAsync();
        var objRes = JsonConvert.DeserializeObject<ResponseOut<UserDto>>(response);
        _mapper.Map(objRes!.Data, this);
    }

    public async IAsyncEnumerable<string> GetAreaCodes()
    {
        var response = await _client!.GetFromJsonAsync<ResponseOut<List<string>>>("api/user/getareacodes");
        foreach (var areaCode in response!.Data!)
        {
            yield return areaCode;   
        }
    }
    
}