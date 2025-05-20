using Microsoft.EntityFrameworkCore;
using TaskAssignment.Domain;
using TaskAssignment.Domain.Exceptions;

namespace TaskAssignment.Api.Shared
{
    public static class ErrorResultDtoFactory
    {
        public static ErrorResultDto Create(Exception exception, string errorLogId)
        {
            if (exception is UserException ue)
            {
                return new ErrorResultDto(ue, errorLogId);
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                return new ErrorResultDto(new UserException("The functionality has encountered temporary problem. Please try again.", TaskAssignmentErrorCodes.Other), errorLogId);
            }

            return new ErrorResultDto(new UserException($"The functionality has encountered problem. Please contact the support. Error Id: {errorLogId}", TaskAssignmentErrorCodes.ServerError), errorLogId);
        }
    }
}
