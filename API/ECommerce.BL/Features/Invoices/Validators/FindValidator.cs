using System.Linq;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Invoices.Validators;

public class FindInvoiceValidator : AbstractValidator<FindInvoiceRequest>
{
    private readonly IStringLocalizer<FindInvoiceValidator> _localizer;

    public FindInvoiceValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindInvoiceValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Invoice]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Invoice]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Invoices.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Invoice]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
