using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Orders.Validators;

public class GetAllOrderValidator : AbstractValidator<GetAllOrderRequest>
{
    private readonly IStringLocalizer<GetAllOrderValidator> _localizer;

    public GetAllOrderValidator(IStringLocalizer<GetAllOrderValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllOrderValidator>(_localizer));
    }
}
