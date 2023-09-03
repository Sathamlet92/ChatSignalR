using System.Net.Http.Json;
using BlazingChat.Shared.Models.DTOs;
using AutoMapper;

namespace BlazingChat.Service.ViewsModels;

public class ContactVM : IContactVM
{
    public long ContactId { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string  AreaCode  { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? LastMessage { get; set; } = string.Empty;
    public string? UrlImage { get; set; }

    private readonly HttpClient? _client;
    private readonly IMapper? _mapper;

    public ContactVM()
    {
    }
    public ContactVM(HttpClient client, IMapper mapper) : this()
    {        
        _client = client;
        _mapper = mapper;
    }
    public async IAsyncEnumerable<IContactVM> GetContacts(long idUser)
    {
        var resultReq = await _client!.GetFromJsonAsync<List<ContactDto>>($"api/user/contacts/{idUser}");

        foreach (var contact in resultReq!)
        {
            yield return _mapper!.Map<IContactVM>(contact);           
        }            
    }
}