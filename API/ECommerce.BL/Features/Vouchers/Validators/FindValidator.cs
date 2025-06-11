using System.Linq;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators;

public class FindVoucherValidator : AbstractValidator<FindVoucherRequest>
{
    private readonly IStringLocalizer<FindVoucherValidator> _localizer;

    public FindVoucherValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindVoucherValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;
        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Voucher]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Voucher]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Vouchers.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Voucher]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
