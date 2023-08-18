using System.ComponentModel.DataAnnotations;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Shared.Models.DTOs;
using Microsoft.AspNetCore.Components;
namespace BlazingChat.Components.Chat;

public partial class Chat
{
    [Parameter]
    public long IdContact { get; set; }
    private List<IContactVM> _contactList;

    private readonly IContactVM _contact;

    public Chat(IContactVM contact)
    {
        _contact = contact;
        _contactList = new();
    }

    protected override async Task OnInitializedAsync()
    {
        await foreach (var contact in _contact.GetContacts())
        {
            _contactList.Add(contact);
            StateHasChanged();
        }    
    }
}
