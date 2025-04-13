using System.Linq;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators
{
    public class ToggleActiveVoucherValidator : AbstractValidator<ToggleActiveVoucherRequest>
    {
        private readonly IStringLocalizer<ToggleActiveVoucherValidator> _localizer;

        public ToggleActiveVoucherValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveVoucherValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Vouchers.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Voucher]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
