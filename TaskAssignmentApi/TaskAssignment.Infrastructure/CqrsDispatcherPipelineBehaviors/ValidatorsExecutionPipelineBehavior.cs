﻿using FluentValidation;
using MediatR;
using TaskAssignment.Application.Interfaces;
using TaskAssignment.Domain;
using TaskAssignment.Domain.Exceptions;
using TaskAssignment.Infrastructure.Shared;

namespace TaskAssignment.Infrastructure.CqrsDispatcherPipelineBehaviors
{
    /*
     * [DESC]
     * We are using MediatR's PipelineBehavior to run validations against handler's input container.
     * The validation mechanism is provided by FluentValidation library.
     */
    public sealed class ValidatorsExecutionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IHandlerContext _handlerContext;

        public ValidatorsExecutionPipelineBehavior(IEnumerable<IValidator<TRequest>> validators, IHandlerContext handlerContext)
        {
            _validators = validators;
            _handlerContext = handlerContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any() || _handlerContext.IsDontUseCommandValidator)
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResultList = await _validators.SelectAsync(async x => await x.ValidateAsync(context));

            List<string> errors = validationResultList.Where(x => !x.IsValid)
                                             .SelectMany(x => x.Errors)
                                             .Where(x => !string.IsNullOrWhiteSpace(x?.ErrorMessage))
                                             .Select(x => x.ErrorMessage)
                                             .ToList();

            if (errors.Any())
            {
                throw new UserException(errors, TaskAssignmentErrorCodes.DataNotValid);
            }

            return await next();
        }
    }
}
