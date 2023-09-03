using BlazingChat.Service.ViewsModels;
using Microsoft.AspNetCore.Components;

namespace BlazingChat.Client.Components;

public partial class CardProfile
{
    [Parameter, EditorRequired]
    public IContactVM? Contact {get; set;}

    [Parameter, EditorRequired]
    public EventCallback<long> SelectContact { get; set; }

    private Task ChangeChat()
    {
        return SelectContact.InvokeAsync(Contact!.ContactId);
    }
}
