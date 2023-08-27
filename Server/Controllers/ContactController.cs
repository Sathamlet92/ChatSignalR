using Microsoft.AspNetCore.Mvc;
using BlazingChat.Shared.Models.DTOs;
using BlazingChat.Server.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazingChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ContactController : ControllerBase
    {
        private readonly IDbContextFactory<ChatContext> _factoryContext;

        public ContactController(IDbContextFactory<ChatContext> factoryContext)
        {
            _factoryContext = factoryContext;
        }

        // [HttpGet]
        // public async Task<List<ContactDto>> GetContactsAsync([FromRoute]long idUser)
        // {
        //     using (var context = await _factoryContext.CreateDbContextAsync())
        //     {     
        //         return await context.Contacts!.Where(con => con.PrincipalUserId.Equals(idUser))
        //             .Include(con => con.ContactUser)
        //             .Include(c => c.ContactUser.Emails)
        //             .Include(c => c.ContactUser.Phones).
        //             Select(c => new ContactDto
        //             {
        //                 ContactId = c.ContactId,
        //                 AreaCode = c.ContactUser.Phones.FirstOrDefault(p => p.UserId == c.ContactUser.UserId)!.AreaCode,
        //                 FirstName = c.ContactName,
        //                 SecondName = c.ContactUser.SecondName,
        //                 LastName = c.ContactUser.LastName!,
        //                 Phone = c.ContactUser.Phones.FirstOrDefault(p => p.UserId == c.ContactUser.UserId)!.Tel,
        //                 LastSecondName = c.ContactUser.SecondName,
        //                 Email = c.ContactUser.Emails.FirstOrDefault(e => e.HasPrincipal!.Value)!.EmailAddress
        //             }).ToListAsync();  
        //     }            
        // }
    }
}
