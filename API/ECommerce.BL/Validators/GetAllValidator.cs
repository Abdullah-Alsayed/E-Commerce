using ECommerce.BLL.Request;
using ECommerce.Core;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Validators;

public class GetAllValidator<T> : AbstractValidator<BaseGridRequest>
{
    private readonly IStringLocalizer<T> _localizer;

    public GetAllValidator(IStringLocalizer<T> localizer)
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
