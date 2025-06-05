using System.Linq;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sliders.Validators
{
    public class ToggleActiveSliderValidator : AbstractValidator<ToggleActiveSliderRequest>
    {
        private readonly IStringLocalizer<ToggleActiveSliderValidator> _localizer;

        public ToggleActiveSliderValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveSliderValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Sliders.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Slider]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
