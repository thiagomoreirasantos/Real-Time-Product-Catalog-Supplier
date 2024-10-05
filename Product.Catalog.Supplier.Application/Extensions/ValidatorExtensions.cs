using FluentResults;
using FluentValidation.Results;
using Product.Catalog.Supplier.Application.Errors;

namespace Product.Catalog.Supplier.Application.Extensions
{
    public static class ValidatorExtensions
    {
        public static Result ToFluentResult(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return Result.Ok();
            }

            return Result.Fail(validationResult.Errors.Select(f => new ValidationError(f.ErrorMessage)));
        }
    }
}
