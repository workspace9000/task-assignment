using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskAssignment.Api.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected ApiControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
