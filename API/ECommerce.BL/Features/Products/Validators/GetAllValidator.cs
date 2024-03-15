using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Products.Validators;

public class GetAllProductValidator : AbstractValidator<GetAllProductRequest>
{
    private readonly IStringLocalizer<GetAllProductValidator> _localizer;

    public GetAllProductValidator(IStringLocalizer<GetAllProductValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllProductValidator>(_localizer));
    }
}
