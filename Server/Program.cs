using System.Text.Json.Serialization;
using BlazingChat.Server.Context;
using BlazingChat.Service.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
