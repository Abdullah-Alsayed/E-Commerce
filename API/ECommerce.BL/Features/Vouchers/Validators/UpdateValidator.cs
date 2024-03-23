using System.Linq;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vouchers.Validators;

public class UpdateVoucherValidator : AbstractValidator<UpdateVoucherRequest>
{
    private readonly IStringLocalizer<UpdateVoucherValidator> _localizer;

    public UpdateVoucherValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateVoucherValidator> localizer
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
            )
            .Must(
                (req, name) =>
                {
                    return !context.Vouchers.Any(x =>
                        x.Name.ToLower() == req.Name.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Name]} {_localizer[Constants.MessageKeys.Exist]}"
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
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, int.MaxValue].ToString())
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 0].ToString());
    }
}
