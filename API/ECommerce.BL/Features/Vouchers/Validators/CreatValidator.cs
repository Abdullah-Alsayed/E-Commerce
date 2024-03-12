using System.Linq;
using Azure;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators
{
    public class CreateVoucherValidator : AbstractValidator<CreateVoucherRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateVoucherValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateVoucherValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Name)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Value)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .LessThan(int.MaxValue)
                .WithMessage(x =>
                    _localizer[Constants.MessageKeys.MaxNumber, int.MaxValue].ToString()
                )
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 1].ToString());

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Voucher.Any(x =>
                        x.Name.ToLower() == req.Name.ToLower() && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Voucher]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
