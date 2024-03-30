using System.Linq;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Governorates.Validators;

public class FindGovernorateValidator : AbstractValidator<FindGovernorateRequest>
{
    private readonly IStringLocalizer<FindGovernorateValidator> _localizer;

    public FindGovernorateValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindGovernorateValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Governorates.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
