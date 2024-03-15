using System.Linq;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators
{
    public class DeleteVoucherValidator : AbstractValidator<DeleteVoucherRequest>
    {
        private readonly IStringLocalizer<CreateVoucherValidator> _localizer;

        public DeleteVoucherValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateVoucherValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Vouchers.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Voucher]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
