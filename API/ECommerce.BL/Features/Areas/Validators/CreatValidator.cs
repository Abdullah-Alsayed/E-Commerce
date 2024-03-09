using System.Linq;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators
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
                    return context.Governorates.Any(x =>
                        x.ID == req.GovernorateID && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Governorate]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Areas.Any(x =>
                        x.NameEN == req.NameEN
                        || x.NameAR == req.NameAR && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Area]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
