using BlazingChat.Client.Models.Configs;
using BlazingChat.Shared.Models.Reponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlazingChat.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ConfigurationController : ControllerBase
{
    private readonly Firebase _firebase;

    public ConfigurationController(IOptions<Firebase> firebase)
    {
        _firebase = firebase.Value;
    }

    [HttpGet("Firebase")]
    public async Task<IActionResult> Firebase()
    {
        var objResponse = _firebase;
        var response = ResponseOut<Firebase>.CreateResponse(true, "Configuracin recuperada correctamente", objResponse);
        return await Task.FromResult(Ok(response.Data));
    }

    public enum TypeConfiguration
    {
        Firebase
    }
}