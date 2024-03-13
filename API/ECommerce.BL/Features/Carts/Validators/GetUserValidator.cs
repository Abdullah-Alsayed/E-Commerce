using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Carts.Validators;

public class GetUserValidator : AbstractValidator<GetUserCartRequest>
{
    private readonly IStringLocalizer<GetUserValidator> _localizer;

    public GetUserValidator(IStringLocalizer<GetUserValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetUserValidator>(_localizer));
    }
}
