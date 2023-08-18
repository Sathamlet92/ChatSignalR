using BlazingChat.Service.ViewsModels;
using Microsoft.AspNetCore.Components;

namespace BlazingChat.Components.Card;

public partial class CardProfile
{
    [Parameter, EditorRequired]
    public IContactVM? Contact {get; set;}
    private bool _selectChat = false;
}
