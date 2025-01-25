using ECommerce.BLL.Features.Units.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Units.Validators;

public class GetAllUnitValidator : AbstractValidator<GetAllUnitRequest>
{
    private readonly IStringLocalizer<GetAllUnitValidator> _localizer;

    public GetAllUnitValidator(IStringLocalizer<GetAllUnitValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllUnitValidator>(_localizer));
    }
}
