using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserCrud.API.Responses;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.Administrator;
using UserCrud.Application.Notifications;

namespace UserCrud.API.Controllers;

public class AdministratorController : MainController
{
    private readonly IAdministratorService _administratorService;

    public AdministratorController(INotificator notificator, IAdministratorService administratorService) :
        base(notificator)
    {
        _administratorService = administratorService;
    }

    [HttpPost]
    [SwaggerOperation("Administrator login")]
    [ProducesResponseType(typeof(AdministratorTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] AdministratorLoginDto dto)
    {
        var token = _administratorService.Login(dto);
        return CustomResponse(token);
    }
}