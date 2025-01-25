using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Colors.Validators;

public class GetAllColorValidator : AbstractValidator<GetAllColorRequest>
{
    private readonly IStringLocalizer<GetAllColorValidator> _localizer;

    public GetAllColorValidator(IStringLocalizer<GetAllColorValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllColorValidator>(_localizer));
    }
}
