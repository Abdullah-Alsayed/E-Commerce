using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Futures.Governorates.Validators;

public class GetAllGovernorateValidator : AbstractValidator<GetAllGovernorateRequest>
{
    private readonly IStringLocalizer<GetAllGovernorateValidator> _localizer;

    public GetAllGovernorateValidator(IStringLocalizer<GetAllGovernorateValidator> localizer)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.SearchFor)
            .MaximumLength(250)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 250].ToString());

        RuleFor(req => req.SearchBy)
            .MaximumLength(250)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 250].ToString());

        RuleFor(req => req.SortBy)
            .MaximumLength(250)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 250].ToString());
    }
}
