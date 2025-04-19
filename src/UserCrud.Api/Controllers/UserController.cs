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
    [SwaggerOperation(Summary = "Create a new user", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var user = await _userService.Create(dto);
        return CustomResponse(user);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a user", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userService.Update(id, dto);
        return CustomResponse(user);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a user", Tags = new[] { "Users" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get a user by id", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetById(id);
        return CustomResponse(user);
    }

    [HttpGet("get-by-email/{email}")]
    [SwaggerOperation(Summary = "Get a user by email", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await _userService.GetByEmail(email);
        return CustomResponse(user);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all users", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return CustomResponse(users);
    }
}