using System.Linq;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Expenses.Validators
{
    public class DeleteExpenseValidator : AbstractValidator<DeleteExpenseRequest>
    {
        private readonly IStringLocalizer<CreateExpenseValidator> _localizer;

        public DeleteExpenseValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateExpenseValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Expenses.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Expense]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
