using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sliders.Validators;

public class GetAllSliderValidator : AbstractValidator<GetAllSliderRequest>
{
    private readonly IStringLocalizer<GetAllSliderValidator> _localizer;

    public GetAllSliderValidator(IStringLocalizer<GetAllSliderValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllSliderValidator>(_localizer));
    }
}
