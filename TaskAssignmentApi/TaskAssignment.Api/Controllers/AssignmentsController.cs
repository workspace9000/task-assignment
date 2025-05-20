using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskAssignment.Api.Shared;
using TaskAssignment.Application.Assignments;

namespace TaskAssignment.Api.Controllers;

public class AssignmentsController : ApiControllerBase
{
    public AssignmentsController(IMediator mediator) : base(mediator) { }

    [HttpGet("assigned-for-user")]
    public async Task<ActionResult<List<ListAssignedTasksForUserItem>>> GetAssignedTasksForUser(
        [FromQuery] ListAssignedTasksForUserQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut("assign-for-user")]
    public async Task<IActionResult> AssignTasks([FromBody] AssignTasksCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}