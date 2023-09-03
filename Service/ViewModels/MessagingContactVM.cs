namespace BlazingChat.Service.ViewsModels;
public class MessagingContactVM
{
    public string ContactId { get; set; } = null!;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? UrlPicture { get; set; }
}