using System.Linq;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Brands.Validators
{
    public class ToggleAvtiveBrandValidator : AbstractValidator<ToggleAvtiveBrandRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveBrandValidator> _localizer;

        public ToggleAvtiveBrandValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleAvtiveBrandValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Brands.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Brand]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
