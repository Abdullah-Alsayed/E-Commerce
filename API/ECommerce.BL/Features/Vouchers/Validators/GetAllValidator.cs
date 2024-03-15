using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators;

public class GetAllVoucherValidator : AbstractValidator<GetAllVoucherRequest>
{
    private readonly IStringLocalizer<GetAllVoucherValidator> _localizer;

    public GetAllVoucherValidator(IStringLocalizer<GetAllVoucherValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllVoucherValidator>(_localizer));
    }
}
