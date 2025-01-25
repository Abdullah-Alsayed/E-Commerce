using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Carts.Validators;

public class GetAllCartValidator : AbstractValidator<GetAllCartRequest>
{
    private readonly IStringLocalizer<GetAllCartValidator> _localizer;

    public GetAllCartValidator(IStringLocalizer<GetAllCartValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllCartValidator>(_localizer));
    }
}
