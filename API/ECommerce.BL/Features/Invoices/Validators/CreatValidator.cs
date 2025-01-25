using System.Linq;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Invoices.Validators
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateInvoiceValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateInvoiceValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Orders.Any(x =>
                        x.ID == req.OrderID && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Order]} {_localizer[Constants.MessageKeys.NotExist]}"
                );
        }
    }
}
