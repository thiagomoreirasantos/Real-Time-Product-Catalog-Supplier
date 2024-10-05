using FluentResults;
using System.Net;

namespace Product.Catalog.Supplier.Application.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string message)
            : base(message)
        {
            WithMetadata("StatusCode", (int)HttpStatusCode.BadRequest);
        }
    }
}
