using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Governorates.Validators;

public class GetAllGovernorateValidator : AbstractValidator<GetAllGovernorateRequest>
{
    private readonly IStringLocalizer<GetAllGovernorateValidator> _localizer;

    public GetAllGovernorateValidator(IStringLocalizer<GetAllGovernorateValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req)
            .SetValidator(new GetAllValidator<GetAllGovernorateValidator>(_localizer));
    }
}
