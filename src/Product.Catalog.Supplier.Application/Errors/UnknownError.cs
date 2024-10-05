using FluentResults;
using System.Net;

namespace Product.Catalog.Supplier.Application.Errors
{
    public class UnknownError : Error
    {
        public UnknownError(string message)
            : base(message)
        {
            WithMetadata("StatusCode", (int)HttpStatusCode.InternalServerError);
        }
    }
}
