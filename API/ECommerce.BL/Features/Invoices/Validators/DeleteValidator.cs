using System.Linq;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Invoices.Validators
{
    public class DeleteInvoiceValidator : AbstractValidator<DeleteInvoiceRequest>
    {
        private readonly IStringLocalizer<CreateInvoiceValidator> _localizer;

        public DeleteInvoiceValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateInvoiceValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Invoices.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Invoice]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
