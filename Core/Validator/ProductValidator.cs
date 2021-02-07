using FluentValidation;
using InventoryManager.API.Data.Models;

namespace InventoryManager.API.Core.Validator
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.Description).NotEmpty();
            RuleFor(product => product.Quantity).NotEmpty();
        }
    }
}
