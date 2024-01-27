using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Futures.Areas.Validators;

public class GetAllAreaValidator : AbstractValidator<GetAllAreaRequest>
{
    private readonly IStringLocalizer<GetAllAreaValidator> _localizer;

    public GetAllAreaValidator(IStringLocalizer<GetAllAreaValidator> localizer)
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
