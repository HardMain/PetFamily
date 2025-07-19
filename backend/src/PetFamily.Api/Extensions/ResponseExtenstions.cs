using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Api.Envelops;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Api.Extensions
{
    public static class ResponseExtenstions
    {
        public static ActionResult ToResponse(this UnitResult<Error> result)
        {
            if (result.IsSuccess)
                return new OkResult();

            var statusCode = result.Error!.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var envelop = Envelop.Error(result.Error);

            return new ObjectResult(envelop)
            {
                StatusCode = statusCode
            };
        }

        public static ActionResult<T> ToResponse<T>(this Result<T, Error> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(Envelop.Ok(result.Value));

            var statusCode = result.Error!.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var envelop = Envelop.Error(result.Error);

            return new ObjectResult(envelop)
            {
                StatusCode = statusCode 
            };
        }
    }
}
