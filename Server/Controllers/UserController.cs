using Microsoft.AspNetCore.Mvc;
using BlazingChat.Server.Models.Entities;
using BlazingChat.Server.Context;
using Microsoft.EntityFrameworkCore;
using BlazingChat.Shared.Models.DTOs;

namespace BlazingChat.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IDbContextFactory<ChatContext> _factoryContext;
    public UserController(IDbContextFactory<ChatContext> factoryContext)
    {
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
    public async Task<List<ContactDto>> GetContactsAsync(long idUser)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {    
            var some1 = await context.Contacts!.ToListAsync();
            var some =  await context.Contacts!.Where(con => con.PrincipalUserId.Equals(idUser)).ToListAsync();
            return await context.Contacts!.Where(con => con.PrincipalUserId.Equals(idUser))
                .Include(con => con.ContactUser)
                .Include(c => c.ContactUser.Emails)
                .Include(c => c.ContactUser.Phones).
                Select(c => new ContactDto
                {
                    FirstName = c.ContactName,
                    SecondName = c.ContactUser.SecondName,
                    LastName = c.ContactUser.LastName!,
                    Phone = c.ContactUser.Phones.FirstOrDefault(p => p.UserId == c.ContactUser.UserId)!.Phone1,
                    LastSecondName = c.ContactUser.SecondName,
                    Email = c.ContactUser.Emails.FirstOrDefault(e => e.HasPrincipal!.Value)!.EmailAddress
                }).ToListAsync();  
        }            
    }

    [HttpPut]
    public async Task<UserDto> UpdateUser(UserDto user)
    {
        using (var context = await _factoryContext.CreateDbContextAsync())
        {
            var entity = await context.Users!.Include(u => u.Emails).Include(u => u.Phones).FirstOrDefaultAsync(u => u.UserId.Equals(user.UserId));
            entity!.AboutMe = user.AboutMe ?? entity.AboutMe;
            entity.ProfilePictureUrl = user.ProfilePictureUrl ?? entity.ProfilePictureUrl;
            entity.LastName = string.IsNullOrEmpty(user.LastName) ? entity.LastName : string.IsNullOrEmpty(user.SecondLastName) ? user.LastName : $"{user.LastName} {user.SecondLastName}"; 
            entity.SecondName = user.SecondName ?? entity.SecondName;
            entity.FirstName = user.FirstName ?? entity.FirstName;
            entity.ProfilePictureUrl = user.ProfilePictureUrl ?? entity.ProfilePictureUrl;
            entity.Emails.FirstOrDefault(e => e.UserId.Equals(entity.UserId) && e.HasPrincipal!.Value)!.EmailAddress = user.EmailAddress ?? entity.Emails.FirstOrDefault(e => e.UserId.Equals(entity.UserId))!.EmailAddress;
            entity.Phones.FirstOrDefault(e => e.UserId.Equals(entity.UserId))!.Phone1 = user.Phone ?? entity.Phones.FirstOrDefault(e => e.UserId.Equals(entity.UserId))!.Phone1;
            context.Update(entity);
            await context.SaveChangesAsync();
            return user;
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
                Phone = u.Phones.FirstOrDefault(p => p.UserId == u.UserId)!.Phone1,
                EmailAddress = u.Emails.FirstOrDefault(e => e.HasPrincipal!.Value)!.EmailAddress,
                Message = "Información recuperada correctamente"

            }).First();
        }        
    }
}

