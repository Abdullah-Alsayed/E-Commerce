using System.Linq;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Products.Validators
{
    public class ToggleActiveProductValidator : AbstractValidator<ToggleActiveProductRequest>
    {
        private readonly IStringLocalizer<ToggleActiveProductValidator> _localizer;

        public ToggleActiveProductValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveProductValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Products.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
