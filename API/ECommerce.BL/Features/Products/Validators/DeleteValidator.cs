using System.Linq;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Products.Validators
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
    {
        private readonly IStringLocalizer<CreateProductValidator> _localizer;

        public DeleteProductValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateProductValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Products.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
