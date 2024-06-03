using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserCrud.API.Responses;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.Auth;
using UserCrud.Application.Notifications;

namespace UserCrud.API.Controllers;

public class AuthController : MainController
{
    private readonly IAuthService _authService;

    public AuthController(INotificator notificator, IAuthService authService) : base(notificator)
    {
        _authService = authService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Login.", Tags = new[] { "Autenticação" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _authService.Login(dto);
        return CustomResponse(token);
    }
}