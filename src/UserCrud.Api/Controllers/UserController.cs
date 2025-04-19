using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserCrud.Api.Responses;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.User;
using UserCrud.Application.Notifications;

namespace UserCrud.Api.Controllers;

[Authorize]
public class UserController : MainController
{
    private readonly IUserService _userService;

    public UserController(INotificator notificator, IUserService userService) : base(notificator)
    {
        _userService = userService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Criar um usuário.", Tags = new[] { "Administração - Usuários" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var createUser = await _userService.Create(dto);
        return CustomResponse(createUser);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar um usuário.", Tags = new[] { "Administração - Usuários" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var updateUser = await _userService.Update(id, dto);
        return CustomResponse(updateUser);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletar um usuário.", Tags = new[] { "Administração - Usuários" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("Obter-Por-Id/{id}")]
    [SwaggerOperation(Summary = "Obter um usuário pelo id.", Tags = new[] { "Administração - Usuários" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getUser = await _userService.GetById(id);
        return CustomResponse(getUser);
    }

    [HttpGet("Obter-Por-Email/{email}")]
    [SwaggerOperation(Summary = "Obter um usuário pelo email.", Tags = new[] { "Administração - Usuários" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var getUser = await _userService.GetByEmail(email);
        return CustomResponse(getUser);
    }

    [HttpGet("Obter-Todos")]
    [SwaggerOperation(Summary = "Obter todos os usuários.", Tags = new[] { "Administração - Usuários" })]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var getUserList = await _userService.GetAll();
        return CustomResponse(getUserList);
    }
}