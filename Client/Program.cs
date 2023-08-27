using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazingChat.Client;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Service.Mappings;
using Microsoft.AspNetCore.Components.Authorization;
using BlazingChat.Service.Security;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton(builder.HostEnvironment);


builder.Services.AddAutoMapper(cfn => 
{
    cfn.AddProfile<ContactProfile>();
    cfn.AddProfile<UserProfile>();
    cfn.AddProfile<SettingsProfile>();
});
builder.Services.AddMudServices();
builder.Services.AddTransient(sp => 
                new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<IContactVM, ContactVM>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});
builder.Services.AddHttpClient<ILoginVM, LoginVM>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});
builder.Services.AddHttpClient<IProfileVM, ProfileVM>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});
builder.Services.AddHttpClient<ISettingsVM, SettingsVM>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});
builder.Services.AddScoped<AuthenticationStateProvider, ChatAuthenticationStateProvider>();

await builder.Build().RunAsync();
