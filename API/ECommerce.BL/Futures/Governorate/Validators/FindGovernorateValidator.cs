using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Governorates.Validators;

public class FindGovernorateValidator : AbstractValidator<FindGovernorateRequest>
{
    private readonly IStringLocalizer<FindGovernorateValidator> _localizer;

    public FindGovernorateValidator(
        Applicationdbcontext context,
        IStringLocalizer<FindGovernorateValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x => Constants.Errors.Register)
            .NotNull()
            .WithMessage(x => Constants.Errors.Register);

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Governorates.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(
                x =>
                    $" {_localizer[Constants.EntitsKeys.Governorates]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
