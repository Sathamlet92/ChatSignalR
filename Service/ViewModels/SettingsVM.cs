using System.Net.Http.Json;
using BlazingChat.Shared.Models.DTOs;

public class SettingsVM : ISettingsVM
{
    public bool Notifications { get; set; }
    public bool DarkTheme { get; set; }

    private readonly HttpClient? _client;

    public SettingsVM()
    {
        
    }
    public SettingsVM( HttpClient client)
    {
        _client = client;
    }

    public async Task GetSettings()
    {
        var result = await _client!.GetFromJsonAsync<SettingsDto?>("api/user/getsettings/1");
        Notifications = result!.Notifications;
        DarkTheme = result!.DarkTheme;
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }
}