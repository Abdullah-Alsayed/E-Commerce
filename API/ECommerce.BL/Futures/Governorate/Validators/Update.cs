using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Governorates.Validators;

public class UpdateGovernorateValidator : AbstractValidator<UpdateGovernorateRequest>
{
    private readonly IStringLocalizer<UpdateGovernorateValidator> _localizer;

    public UpdateGovernorateValidator(
        Applicationdbcontext context,
        IStringLocalizer<UpdateGovernorateValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.NameAR)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(
                x =>
                    $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(
                x =>
                    $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.NameEN)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(
                x =>
                    $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(
                x =>
                    $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.Tax)
            .NotNull()
            .WithMessage(
                x =>
                    $"{_localizer[Constants.EntitsKeys.Tax]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(
                x =>
                    $"{_localizer[Constants.EntitsKeys.Tax]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .LessThan(int.MaxValue)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, int.MaxValue].ToString())
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString());

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Governorates.Any(x => x.ID == req.ID && !x.IsDeleted);
            })
            .WithMessage(
                x =>
                    $" {_localizer[Constants.EntitsKeys.Governorates]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
