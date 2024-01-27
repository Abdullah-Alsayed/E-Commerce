using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Areas.Validators
{
    public class CreateAreaValidator : AbstractValidator<CreateAreaRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateAreaValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateAreaValidator> localizer
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
                .WithMessage(
                    x => _localizer[Constants.MessageKeys.MaxNumber, int.MaxValue].ToString()
                )
                .GreaterThanOrEqualTo(0)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString());

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Areas.Any(
                        x =>
                            x.NameEN == req.NameEN
                            || x.NameAR == req.NameAR && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(
                    x =>
                        $" {_localizer[Constants.EntitsKeys.Areas]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
