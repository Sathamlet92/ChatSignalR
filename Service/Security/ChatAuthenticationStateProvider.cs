using System.Net.Http.Json;
using System.Security.Claims;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Shared.Models.Reponse;
using Microsoft.AspNetCore.Components.Authorization;
namespace BlazingChat.Service.Security;

public class ChatAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _client;

    public ChatAuthenticationStateProvider(HttpClient client)
    {
        _client = client;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await _client.GetFromJsonAsync<ResponseOut<UserDto>>("api/user/getcurrentuser");
        if (currentUser != null && currentUser.Data != null ) 
        { 
            var email = currentUser.Data.Emails.FirstOrDefault(e => e.HasPrincipal!.Value);
            if(email != null)
            {
                 //create a claims
                var claimUserName= new Claim(ClaimTypes.Name, currentUser!.Data!.UserName!);
                var claimEmailAddress = new Claim(ClaimTypes.Email, email.EmailAddress);
                var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Data.UserId));
                //create claimsIdentity
                var claimsIdentity = new ClaimsIdentity(new[] { claimEmailAddress, claimNameIdentifier, claimUserName }, "serverAuth");
                //create claimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
            else 
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            
        }
        else
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}
