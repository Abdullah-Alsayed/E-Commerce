using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Brands.Validators;

public class GetAllBrandValidator : AbstractValidator<GetAllBrandRequest>
{
    private readonly IStringLocalizer<GetAllBrandValidator> _localizer;

    public GetAllBrandValidator(IStringLocalizer<GetAllBrandValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllBrandValidator>(_localizer));
    }
}
