using Microsoft.AspNetCore.Mvc;
using BlazingChat.Server.Context;
using Microsoft.EntityFrameworkCore;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Shared.Models.Reponse;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using BlazingChat.Domain.Models.Entites;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;

namespace BlazingChat.Server.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IDbContextFactory<ChatContext> _factoryContext;
    private readonly IMapper _mapper;
    public UserController(IDbContextFactory<ChatContext> factoryContext, IMapper mapper)
    {
        _mapper = mapper;
        _factoryContext = factoryContext;
    }

    [HttpPost("login")]
    public async Task<ResponseOut<LoginDto>> Login(LoginDto user)
    {
        using(var context = await _factoryContext.CreateDbContextAsync())
        {
            var loggedUserIn = await context.Users!
                .Join(context.Logins!, u => u.UserId, l => l.UserId, (u, l) => new {User = u, Login = l })
                .Where(l => (l.Login.EmailAddres.Equals(user.User) || l.Login.UserName.Equals(user.User)) && l.Login.Password.Equals(user.Password))
                .FirstOrDefaultAsync();
            if(loggedUserIn is not null && loggedUserIn.User is not null)
            {
                var claims = new[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedUserIn.User.UserId.ToString()),
                    new Claim(ClaimTypes.Name, loggedUserIn.Login.UserName),
                    new Claim(ClaimTypes.Email, loggedUserIn.Login.EmailAddres)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "serverAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
            }
            return ResponseOut<LoginDto>.CreateResponse(loggedUserIn is not null, loggedUserIn is not null? "Inicio de sesión satisfactorio" : "El nombre de usuario/correo electronico o contraseñá son incorrectos" );
        }
    }

    [HttpGet("logoutuser")]
    public async Task<ActionResult<ResponseOut<string>>> LogOutUser()
    {
        await HttpContext.SignOutAsync();
        return Ok(ResponseOut<string>.CreateResponse(true, "Sesión cerrada exitosamente", null));
    }

    [HttpGet("getcurrentuser")]
    public async Task<ResponseOut<UserDto>> GetCurrentUser()
    {
        if(User.Identity!.IsAuthenticated)
        {
            using (var context = await _factoryContext.CreateDbContextAsync())
            {
                var currentUser1 = await context.Logins!
                    .Include(l => l.User)
                    .Where(l => l.EmailAddres.Equals(User.FindFirstValue(ClaimTypes.Email)) || l.UserName.Equals(User.FindFirstValue(ClaimTypes.Name)))
                    .Select(l => l.User)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                return ResponseOut<UserDto>.CreateResponse(true, "Recuperado satisfactoriamente", currentUser1);
            }
        }
        return ResponseOut<UserDto>.CreateResponse(false, "Sin usuario autenticado", null);
    }

    [HttpGet("user")]
    public async Task<List<User>> GetUsersAsync()
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            return await context.Users!.Include(u => u.Emails).Include(u => u.Logins).ToListAsync();        
        }
    }
    [HttpGet("contacts/{idUser}")]
    public async Task<List<ContactDto>> GetContactsAsync(long idUser, [FromQuery]bool hasConversation)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            var contacts = await context.Contacts!
                .Where(c => c.PrincipalUserId.Equals(idUser))
                    .Include(c => c.ContactUser)
                        .ThenInclude(u => u.Phones)
                    .Include(c => c.ContactUser)
                        .ThenInclude(u => u.Emails)
                    .Include(c => c.ContactUser)
                        .ThenInclude(u => u.Logins)
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return contacts;       
        }            
    }

    [HttpPut]
    public async Task<ResponseOut<UserDto>> UpdateUser(UserDto user)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            var entity = await context.Users!.FirstOrDefaultAsync(u => u.UserId.Equals(user.UserId));
            
            foreach (var phone in user.Phones)
            {
                var entityPhone = await context.Phones!.FirstOrDefaultAsync(p => p.UserId.Equals(entity!.UserId) && p.PhoneId.Equals(phone.PhoneId));
                entityPhone = _mapper.Map(phone, entityPhone);
                context.Update<Phone>(entityPhone!);         
            }
            foreach (var email in user.Emails)
            {
                var entityEmail = await context.Emails!.FirstOrDefaultAsync(p => p.UserId.Equals(entity!.UserId) && p.EmailId.Equals(email.EmailId));
                entityEmail = _mapper.Map(email, entityEmail);
                context.Update<Email>(entityEmail!);                  
            }

            entity = _mapper.Map(user, entity);

            context.Update<User>(entity!);
            context.Emails!.UpdateRange();
            var result = await context.SaveChangesAsync();
            //await Task.FromResult(entity);
            return ResponseOut<UserDto>.CreateResponse(true, "Actualizado correctamente", user);
        }   
    }

    [HttpGet("getprofile/{idUser}")]
    public async Task<UserDto> GetProfile(long idUser)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            var user = await context.Users!.Where(u => u.UserId.Equals(idUser))
                .Include(u => u.Phones)
                .Include(u => u.Emails)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstAsync();
            return user;
        }        
    }

    [HttpGet("getsettings/{idUser}")]
    public async Task<SettingsDto> GetSettingsAsync(long idUser)
    {
        using(var context = await _factoryContext.CreateDbContextAsync())
        {
            return await context.Users!.Where(u => u.UserId.Equals(idUser)).Select( u => new SettingsDto {DarkTheme = u.DarkTheme, Notifications = u.Notifications}).SingleAsync();
        }
    }

    [HttpPut("updatesettings/{idUser}")]
    public async Task<SettingsDto> UpdateSettingsAsync(long idUser, SettingsDto settings)
    {
        using(var context = await _factoryContext.CreateDbContextAsync())
        {
            var user = await context.Users!.Where(u => u.UserId.Equals(idUser)).Select( u => new SettingsDto {DarkTheme = u.DarkTheme, Notifications = u.Notifications}).SingleAsync();
            await Task.FromResult(user);
            return settings;
        }
    }

    [HttpGet("getareacodes")]
    public async Task<ResponseOut<List<string>>> GetAreaCodesAsync()
    {
        using(var context = await _factoryContext.CreateDbContextAsync())
        {
            var res = await context.AreaCodes!.Select(a => a.CodeNum).ToListAsync();
            return ResponseOut<List<string>>.CreateResponse(res.Any(), "Información recuperada con exito", res);
        }        
    }
    [HttpGet("updatenotificaction")]
    public async Task<ResponseOut<UserDto>> UpdateNotification(long userId, bool hasNotification)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            var user = await context.Users!
                .FirstAsync(u => u.UserId.Equals(userId));
            user.Notifications = hasNotification;
            await context.SaveChangesAsync();
            await Task.FromResult(user);
            return ResponseOut<UserDto>.CreateResponse(true, "Actulizado con exito", _mapper.Map(user, new UserDto()));
        }
    }
    [HttpGet("facebooksignin")]
    public async Task FacebookSignin()
    {
        await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties {RedirectUri = "/profile"});
    }

    [HttpGet("googlesignin")]
    public async Task GoogleSignin()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties {RedirectUri = "/profile"});
    }
}

