using Microsoft.AspNetCore.Mvc;
using BlazingChat.Server.Context;
using Microsoft.EntityFrameworkCore;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Shared.Models.Reponse;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using BlazingChat.Domain.Models.Entites;

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
            return await context.Contacts!
                .Where(c => c.PrincipalUserId.Equals(idUser))
                    .Include(c => c.ContactUser)
                        .ThenInclude(u => u.Phones)
                    .Include(c => c.ContactUser)
                        .ThenInclude(u => u.Emails)
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();            
        }            
    }

    [HttpPut]
    public async Task<ResponseOut<UserDto>> UpdateUser(UserDto user)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            var entity = await context.Users!.Include(u => u.Emails).Include(u => u.Phones).FirstOrDefaultAsync(u => u.UserId.Equals(user.UserId));
            entity = _mapper.Map(user, entity);
            var result = await context.SaveChangesAsync();
            var flag = result > 0;
            await Task.FromResult(entity);
            return ResponseOut<UserDto>.CreateResponse(flag, flag ? "Actualizado correctamente" : "No se pudo actualizar", user);
        }   
    }

    [HttpGet("getprofile/{idUser}")]
    public async Task<UserDto> GetProfile(long idUser)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            return context.Users!.Include(u => u.Emails).Include(u => u.Phones).Where(u => u.UserId.Equals(idUser)).Select(u => new UserDto
            {
                UserId =  u.UserId,
                AboutMe = u.AboutMe,
                ProfilePictureUrl = u.ProfilePictureUrl,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                LastName = u.LastName!.Split()[0],
                SecondLastName = u.LastName!.Split().Count() > 1 ? u.LastName!.Split()[1] : null,
                Phone = u.Phones.FirstOrDefault(p => p.UserId == u.UserId)!.Tel,
                EmailAddress = u.Emails.FirstOrDefault(e => e.HasPrincipal!.Value)!.EmailAddress,
                Message = "Información recuperada correctamente"

            }).First();
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

    [HttpPut("getsettings/{idUser}")]
    public async Task<SettingsDto> UpdateSettingsAsync(long idUser, SettingsDto settings)
    {
        using(var context = await _factoryContext.CreateDbContextAsync())
        {
            var user = await context.Users!.Where(u => u.UserId.Equals(idUser)).Select( u => new SettingsDto {DarkTheme = u.DarkTheme, Notifications = u.Notifications}).SingleAsync();
            await Task.FromResult(user);
            return settings;
        }
    }
}

