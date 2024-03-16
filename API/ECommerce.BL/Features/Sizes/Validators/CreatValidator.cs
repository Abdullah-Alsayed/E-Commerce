using System.Linq;
using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sizes.Validators
{
    public class CreateSizeValidator : AbstractValidator<CreateSizeRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateSizeValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateSizeValidator> localizer
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

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Sizes.Any(x =>
                        x.NameEN == req.NameEN
                        || x.NameAR == req.NameAR && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Size]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
