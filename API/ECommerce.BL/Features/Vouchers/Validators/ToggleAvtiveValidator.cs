using System.Linq;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators
{
    public class ToggleAvtiveVoucherValidator : AbstractValidator<ToggleAvtiveVoucherRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveVoucherValidator> _localizer;

        public ToggleAvtiveVoucherValidator(
            Applicationdbcontext context,
            IStringLocalizer<ToggleAvtiveVoucherValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Voucher.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Voucher]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
