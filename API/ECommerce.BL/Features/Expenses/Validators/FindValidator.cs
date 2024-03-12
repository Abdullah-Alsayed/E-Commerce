using System.Linq;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Expenses.Validators;

public class FindExpenseValidator : AbstractValidator<FindExpenseRequest>
{
    private readonly IStringLocalizer<FindExpenseValidator> _localizer;

    public FindExpenseValidator(
        Applicationdbcontext context,
        IStringLocalizer<FindExpenseValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x => Constants.Errors.Register)
            .NotNull()
            .WithMessage(x => Constants.Errors.Register);

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Expenses.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Expense]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
