using System.Linq;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sliders.Validators
{
    public class DeleteSliderValidator : AbstractValidator<DeleteSliderRequest>
    {
        private readonly IStringLocalizer<CreateSliderValidator> _localizer;

        public DeleteSliderValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateSliderValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Sliders.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Slider]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
