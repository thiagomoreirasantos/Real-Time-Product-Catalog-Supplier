using FluentValidation;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Api.Validation
{
    public class ProductValidator: AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("Product name cannot be empty")
                .MaximumLength(100)
                .WithMessage("Product name cannot be longer than 100 characters");

            RuleFor(product => product.Brand)         
                .NotNull()
                .WithMessage("Product brand cannot be null");

            RuleFor(product => product.AvailableColors) 
                .NotNull()
                .WithMessage("Product available colors cannot be null")
                .Must(colors => colors.Count > 0)
                .WithMessage("Product available colors cannot be empty");

            RuleForEach(product => product.AvailableColors)
                .Must(color => color?.Name != null && Color.IsValidColorName(color.Name))
                .WithMessage("Product available colors must be valid colors");           

            RuleFor(product => product.Metadata)    
                .NotNull()
                .WithMessage("Product metadata cannot be null")
                .Must(metadata => metadata.Count > 0)
                .WithMessage("Product metadata cannot be empty");          

            RuleFor(product => product.ProductionItems)
                .NotNull()
                .WithMessage("Product production items cannot be null")
                .Must(productionItems => productionItems.Count > 0)
                .WithMessage("Product production items cannot be empty");
        }
    }
}