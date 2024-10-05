using FluentValidation;
using Product.Catalog.Supplier.DataContracts;

namespace Product.Catalog.Supplier.Application.Validators
{
    public class MessageEnvelopeValidator : AbstractValidator<ProductDto>
    {
        public MessageEnvelopeValidator() 
        {
            RuleFor(x => x.MessageType).NotEmpty().WithMessage("Missing argument MessageType");
            RuleFor(x => x.CorrelationId).NotEmpty().WithMessage("Missing argument CorrelationId");
            RuleFor(x => x.MessageId).NotEmpty().WithMessage("Missing argument MessageId");
            RuleFor(x => x.Payload).NotEmpty().WithMessage("Missing argument Payload");
            RuleFor(x => x.Timestamp).Must(timestamp => BeAValidTimestamp(timestamp.ToString())).When(x => !string.IsNullOrEmpty(x.Timestamp)).WithMessage("Invalid argument timestamp");
        }

        private static bool BeAValidTimestamp(string timestamp)
        {
            if (timestamp.Contains('.'))
            {
                return ulong.TryParse(timestamp.Split('.')[0], out _);
            }

            return ulong.TryParse(timestamp, out _);
        }
    }
}
