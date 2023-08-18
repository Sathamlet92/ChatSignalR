using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazingChat.Client;
using BlazingChat.Service;
using BlazingChat.Service.ViewsModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IContactVM, ContactVM>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});
builder.Services.AddHttpClient<ILoginService, LoginService>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});
builder.Services.AddHttpClient<IProfileVM, ProfileVM>("BlazingChatClient", client => {client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);});


builder.Services.AddScoped(sp => new HttpClient {  });

await builder.Build().RunAsync();
