using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vendors.Validators;

public class GetAllVendorValidator : AbstractValidator<GetAllVendorRequest>
{
    private readonly IStringLocalizer<GetAllVendorValidator> _localizer;

    public GetAllVendorValidator(IStringLocalizer<GetAllVendorValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllVendorValidator>(_localizer));
    }
}
