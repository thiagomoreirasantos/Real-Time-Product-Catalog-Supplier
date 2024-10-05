using FluentResults;
using System.Net;

namespace Product.Catalog.Supplier.Application.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message)
            : base(message)
        {
            WithMetadata("StatusCode", (int)HttpStatusCode.NotFound);
        }
    }
}
