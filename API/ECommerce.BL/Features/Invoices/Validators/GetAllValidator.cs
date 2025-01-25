using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Invoices.Validators;

public class GetAllInvoiceValidator : AbstractValidator<GetAllInvoiceRequest>
{
    private readonly IStringLocalizer<GetAllInvoiceValidator> _localizer;

    public GetAllInvoiceValidator(IStringLocalizer<GetAllInvoiceValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllInvoiceValidator>(_localizer));
    }
}
