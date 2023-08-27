namespace BlazingChat.Service.ViewsModels;
public interface IContactVM
{
    public long ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; } 
    public string? Phone { get; set; }
    public string  LastMessage { get; set; }  
    public IAsyncEnumerable<IContactVM> GetContacts(long idUser);
}