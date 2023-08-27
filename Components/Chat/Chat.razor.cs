using BlazingChat.Service.ViewsModels;
using Microsoft.AspNetCore.Components;
namespace BlazingChat.Components.Chat;

public partial class Chat
{
    [Parameter]
    public long IdContact { get; set; }
    private List<IContactVM> _contactList;

    [Inject]
    public IContactVM? Contact {get;set;}

    public Chat()
    {
        _contactList = new();
    }

    // protected override async Task OnInitializedAsync()
    // {
    //     await foreach (var contact in Contact!.GetContacts())
    //     {
    //         _contactList.Add(contact);
    //         StateHasChanged();
    //     }    
    // }
}
