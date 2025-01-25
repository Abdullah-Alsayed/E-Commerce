using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators;

public class GetAllAreaValidator : AbstractValidator<GetAllAreaRequest>
{
    private readonly IStringLocalizer<GetAllAreaValidator> _localizer;

    public GetAllAreaValidator(IStringLocalizer<GetAllAreaValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllAreaValidator>(_localizer));
    }
}
