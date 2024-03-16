using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sizes.Validators;

public class GetAllSizeValidator : AbstractValidator<GetAllSizeRequest>
{
    private readonly IStringLocalizer<GetAllSizeValidator> _localizer;

    public GetAllSizeValidator(IStringLocalizer<GetAllSizeValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllSizeValidator>(_localizer));
    }
}
