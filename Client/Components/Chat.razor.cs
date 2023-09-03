using AutoMapper;
using BlazingChat.Service.ViewsModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazingChat.Client.Components;

public partial class Chat
{
    [Parameter]
    public long IdContact { get; set; }

    [CascadingParameter]
    public Task<AuthenticationState>? Authentication { get; set; }    

    [Inject]
    public IContactVM? Contact {get;set;}

    [Inject]
    public NavigationManager? Navigation { get; set; }

    [Inject]
    public IMessagingProfileVM? MessagingProfile { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }

    public EventCallback<long> IdReceive { get; set; }

    public Chat()
    {
        _contactList = new();
    }
    private long _idUser;
    private List<IContactVM>? _contactList;
    private MessagingContactVM? _contact;
    protected override async Task OnInitializedAsync()
    {
        var authState = await Authentication!;
        var user = authState.User;

        if(user.Identity != null && user.Identity.IsAuthenticated)
        {
            var claim = user.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            _idUser = Convert.ToInt64(claim!.Value);
            await foreach (var contact in Contact!.GetContacts(_idUser))
            {
                _contactList!.Add(contact);
                StateHasChanged();
            }
            await MessagingProfile!.GetProfile(_idUser);
            if(IdContact != 0)
            {
                var contactFind = _contactList!.FirstOrDefault(c => c.ContactId.Equals(IdContact));
                _contact = Mapper!.Map(contactFind, _contact);
            }            
        }        
    }
    private async Task GetId(long idContact)
    {
        var contact = _contactList!.FirstOrDefault(c => c.ContactId.Equals(idContact));
        _contact = Mapper!.Map(contact, _contact);
        await IdReceive.InvokeAsync(idContact);
        IdContact = idContact;
        StateHasChanged();
    }
}
