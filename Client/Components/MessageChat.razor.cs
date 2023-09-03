using System.Security.Claims;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazingChat.Client.Components;

public partial class MessageChat
{
    public string? FromUserId { get; set; }
    public string? MessageText { get; set; }
    public string? ToUserId { get; set; }
    [Parameter]
    public Messages? Message { get; set; }

    [Parameter, EditorRequired]
    public MessagingContactVM? Contact{ get; set; } 

    [Parameter, EditorRequired]
    public IMessagingProfileVM? MessagingProfile { get; set; }

    [Inject]
    public NavigationManager? Navigation { get; set; }

    [CascadingParameter]
    public Task<AuthenticationState>? Authentication { get; set; }

    private List<Messages> _messages { get; set; } = new();

    private  HubConnection? _hubConnection;

    protected override async Task OnInitializedAsync()
    {
        var authState = await Authentication!;
        var user = authState.User;

        if(user.Identity != null && user.Identity.IsAuthenticated)
        {
            var claim = user.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            FromUserId = claim!.Value;

            Message = Message ?? new Messages();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation!.ToAbsoluteUri("/chathub"))
                .Build();
            _hubConnection.On<Messages>("ReceiveMessage", (message) =>
            {
                _messages.Add(message);
                StateHasChanged();
            });
            await _hubConnection.StartAsync();
        }        
    }

    private async Task PressEnter(KeyboardEventArgs e)
    {
        if(e.Code.Equals("Enter"))
            await Send();
    }

    public async Task Send()
    {
        Messages message = new Messages();
        message.ToUserId = Contact!.ContactId;
        message.FromUserId = MessagingProfile!.UserId;
        message.MessageText = MessageText;
        await _hubConnection!.SendAsync("SendMessageAsync",message);
        MessageText = string.Empty;
    }
}
