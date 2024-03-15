using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Expenses.Validators;

public class GetAllExpenseValidator : AbstractValidator<GetAllExpenseRequest>
{
    private readonly IStringLocalizer<GetAllExpenseValidator> _localizer;

    public GetAllExpenseValidator(IStringLocalizer<GetAllExpenseValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllExpenseValidator>(_localizer));
    }
}
