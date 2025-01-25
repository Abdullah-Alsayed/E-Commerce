using System.Linq;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sliders.Validators;

public class FindSliderValidator : AbstractValidator<FindSliderRequest>
{
    private readonly IStringLocalizer<FindSliderValidator> _localizer;

    public FindSliderValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindSliderValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Slider]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Slider]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );
        RuleFor(req => req)
            .Must(req =>
            {
                return context.Sliders.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Slider]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
