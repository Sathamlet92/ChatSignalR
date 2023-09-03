using System.Security.Claims;
using BlazingChat.Client.Models.Configs;
using BlazingChat.Service.ViewsModels;
using BlazingChat.Shared.Models.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;

namespace BlazingChat.Client.Pages;

public partial class Profile
{
    private byte[]? _valueInput;
    private int progressPercentage = 0;
    private string progressPercentageString => $"{progressPercentage}%";
    private List<List<AreaCodeVM>>? _areaCodes = new();
    private string IdOption = string.Empty;
    private string? _previewImage;
    private string _messageTypeImage = string.Empty;
    private const long MAX_SIZE_FILE = 1024000 * 100;
    private ImageDto? _image; 

    private MudFileUpload<IBrowserFile>? _inputRef;

    [Inject]
    public IProfileVM? ProfileVM { get; set; }

    [Inject]
    public IJSRuntime? JSInterop { get; set; }

    [Inject]
    public HttpClient? Client { get; set; }

    [CascadingParameter]
    public Task<AuthenticationState>? Authentication { get; set; }
    private string _phone = string.Empty;

    // protected override async Task OnAfterRenderAsync(bool firstRender) {
    //     if (firstRender)
    //     {
    //         //jsModule = await JSInterop.InvokeAsync<IJSObjectReference>("import", "./wwwroot/js/interop.js");
    //     }
    //     await base.OnAfterRenderAsync(firstRender);
    // }

    protected override async Task OnInitializedAsync() 
    {
        var authState = await Authentication!;
        var user = authState.User;
        
        if(user.Identity != null && user.Identity.IsAuthenticated)
        {
            var claim = user.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            ProfileVM!.UserId = Convert.ToInt64(claim!.Value);
            var sessionStoredProfile = await JSInterop!.InvokeAsync<string>("MyLib.getProfileSession", "profile");
            if(string.IsNullOrEmpty(sessionStoredProfile))
            {
                await ProfileVM.GetProfile(ProfileVM.UserId);
                
                var jsonSerializer = JsonConvert.SerializeObject(ProfileVM);

                await JSInterop!.InvokeVoidAsync("MyLib.saveProfileSession", new{Key = "profile", Value = jsonSerializer});
            }
            else
            {
                var resultSerialized = JsonConvert.DeserializeObject<ProfileVM>(sessionStoredProfile);
                ProfileVM.AreaCodes = resultSerialized?.AreaCodes;
                ProfileVM.Phones = resultSerialized!.Phones;
                ProfileVM.FirstName = resultSerialized!.FirstName;
                ProfileVM.SecondName = resultSerialized!.SecondName;
                ProfileVM.LastName = resultSerialized!.LastName;
                ProfileVM.SecondLastName = resultSerialized!.SecondLastName;
                ProfileVM.Emails = resultSerialized.Emails;
                ProfileVM.UrlImageProfile = resultSerialized.UrlImageProfile;
            }
            StateHasChanged();            
            var sessionStoredAreaCodes = await JSInterop!.InvokeAsync<string>("MyLib.getProfileSession", "areaCodes");
            if(string.IsNullOrEmpty(sessionStoredAreaCodes))
            {
                foreach (var phone in ProfileVM!.Phones)
                {
                    var listTemp = new List<AreaCodeVM>();
                    await foreach (var areacode in ProfileVM.GetAreaCodes())
                    {                                        
                        var compare = areacode.EndsWith(phone.AreaCode);
                        int id = 1;
                        listTemp.Add(new AreaCodeVM{IdOption = id, AreaCode = areacode, IsSelected = compare});
                        StateHasChanged();
                        ++id;                         
                    }
                    _areaCodes!.Add(listTemp);
                }
                var jsonSerializer = JsonConvert.SerializeObject(_areaCodes);
                await JSInterop!.InvokeVoidAsync("MyLib.saveProfileSession", new{Key = "areaCodes", Value = jsonSerializer});
            }
            else
            {
                _areaCodes = JsonConvert.DeserializeObject<List<List<AreaCodeVM>>>(sessionStoredAreaCodes);
                StateHasChanged();
            }                   
        }        
    }
    
    private async Task UpdateProfile() 
    {          
        if(_image != null)
        {
            _image.ImageName =  $"{ProfileVM!.FirstName} {ProfileVM!.LastName}-Perfil";
            _image.FolderName = $"{ProfileVM!.FirstName} {ProfileVM!.SecondName} {ProfileVM!.LastName} {ProfileVM!.SecondLastName}";
            var configs = await Client!.GetStringAsync("api/configuration/firebase");           
            var jsonOb = JsonConvert.DeserializeObject<Firebase>(configs);
            _previewImage = await JSInterop!.InvokeAsync<string>("MyLib.uploadFile", _image, jsonOb, DotNetObjectReference.Create(this));
            StateHasChanged();
        }
        ProfileVM!.UrlImageProfile = _previewImage ?? ProfileVM.UrlImageProfile;
        await ProfileVM.UpdateProfile();
        _image = null;
        await _inputRef!.ResetAsync();
        StateHasChanged();
    } 

    private async Task InputFile (IBrowserFile e)
    {
        var inputFile = e;
         var fileType = inputFile.ContentType;
        if(!fileType.StartsWith("image"))
        {
            _messageTypeImage = "Solo se permiten imagenes";
            StateHasChanged();
            return;
        }
        using(var st = inputFile.OpenReadStream(MAX_SIZE_FILE))
        {
            var buffer = new byte[inputFile.Size];
            await st.ReadAsync(buffer);
            _valueInput = buffer;
            _image = new()
            {
                ImageData = buffer,
                ContentType = inputFile.ContentType
            };
            _previewImage = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            StateHasChanged();
        }
    }

    [JSInvokable]
    public async void UpdateProgress(double percentage)
    {
        progressPercentage = Convert.ToInt32(percentage);
        StateHasChanged();
        if(progressPercentage == 100)
        {
            await Task.Delay(25);
            progressPercentage++;
            StateHasChanged();
        }
    }    

    private async Task GetProfile() => await ProfileVM!.GetProfile(ProfileVM.UserId);    
}