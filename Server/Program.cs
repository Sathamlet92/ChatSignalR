using System.Text.Json.Serialization;
using BlazingChat.Server.Context;
using BlazingChat.Service.Mappings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");
builder.Services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(opt => 
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddAutoMapper(cfn => 
{
    cfn.AddProfile<ContactProfile>();
    cfn.AddProfile<UserProfile>();
    cfn.AddProfile<SettingsProfile>();
});

var parentDir = Environment.CurrentDirectory;
var connectionString = string.Format(builder.Configuration.GetConnectionString("BlazingChat")!, parentDir.Replace("server", "Server"));
builder.Services.AddDbContextFactory<ChatContext>(opt => opt.UseSqlite(connectionString));
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie()
.AddFacebook(fo => 
{ 
    fo.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
    fo.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
})
.AddGoogle(go => 
{
    go.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    go.ClientSecret =   builder.Configuration["Authentication:Google:ClientSecret"]!;
}); 
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(opt => 
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
