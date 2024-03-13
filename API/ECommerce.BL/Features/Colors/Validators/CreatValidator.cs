using System.Linq;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Colors.Validators
{
    public class CreateColorValidator : AbstractValidator<CreateColorRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateColorValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateColorValidator> localizer
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
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.NameEN)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Value)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Colors.Any(x =>
                        x.NameEN == req.NameEN
                        || x.NameAR == req.NameAR
                        || x.Value == req.Value && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
