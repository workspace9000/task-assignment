using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskAssignment.Api.Shared;
using TaskAssignment.Application.Tasks;

namespace TaskAssignment.Api.Controllers;

public class TasksController : ApiControllerBase
{
    public TasksController(IMediator mediator) : base(mediator) { }

    [HttpGet("available")]
    public async Task<ActionResult<List<ListAvailableTasksForUserItem>>> GetAvailableTasksForUser(
        [FromQuery] ListAvailableTasksForUserQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}