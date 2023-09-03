namespace BlazingChat.Service.ViewsModels;
public interface IContactVM
{
    public long ContactId { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; } 
    public string? Phone { get; set; }
    public string? LastMessage { get; set; }
    public string UserName { get; set; }
    public string? UrlImage { get; set; }
    public IAsyncEnumerable<IContactVM> GetContacts(long idUser);
}