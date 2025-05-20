using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskAssignment.Api.Shared;
using TaskAssignment.Application.Users;

namespace TaskAssignment.Api.Controllers;

public class UsersController : ApiControllerBase
{
    public UsersController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    public async Task<ActionResult<List<ListAllUsersItem>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new ListAllUsersQuery());
        return Ok(users);
    }
}

