namespace BlazingChat.Service.ViewsModels;
public interface IMessagingProfileVM
{
    public string UserId { get; set; }
    public string? UrlPicture { get; set; }    
    public string? Nombre { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }

    public Task GetProfile(long idUser);
}