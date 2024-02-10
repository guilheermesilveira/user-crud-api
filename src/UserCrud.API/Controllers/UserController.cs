using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserCrud.API.Responses;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.User;
using UserCrud.Application.Notifications;

namespace UserCrud.API.Controllers;

[Authorize]
public class UserController : MainController
{
    private readonly IUserService _userService;

    public UserController(INotificator notificator, IUserService userService) : base(notificator)
    {
        _userService = userService;
    }

    [HttpPost]
    [SwaggerOperation("Create a user")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var createUser = await _userService.Create(dto);
        return CustomResponse(createUser);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Update a user")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var updateUser = await _userService.Update(id, dto);
        return CustomResponse(updateUser);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Get by id a user")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getUser = await _userService.GetById(id);
        return CustomResponse(getUser);
    }

    [HttpGet("GetByEmail/{email}")]
    [SwaggerOperation("Get by email a user")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var getUser = await _userService.GetByEmail(email);
        return CustomResponse(getUser);
    }

    [HttpGet("GetAll")]
    [SwaggerOperation("Get all users")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var getUserList = await _userService.GetAll();
        return CustomResponse(getUserList);
    }
}