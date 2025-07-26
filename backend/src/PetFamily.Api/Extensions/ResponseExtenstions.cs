using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Api.Envelopes;

namespace PetFamily.Api.Extensions
{
    public static class ResponseExtenstions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var responseError = new ResponseError(error.Code, error.Message, null);

            var envelop = Envelope.Error([responseError]);

            return new ObjectResult(envelop)
            {
                StatusCode = statusCode
            };
        }
    }
}