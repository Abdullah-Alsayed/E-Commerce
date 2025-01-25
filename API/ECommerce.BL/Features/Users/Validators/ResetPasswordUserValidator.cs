using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators
{
    public class ResetPasswordUserValidator : AbstractValidator<ResetPasswordUserRequest>
    {
        private readonly IStringLocalizer _localizer;

        public ResetPasswordUserValidator(
            ApplicationDbContext context,
            IStringLocalizer<ResetPasswordUserValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Email)
                .NotNull()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .EmailAddress()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.NotValid]}"
                );

            RuleFor(req => req.Token)
                .NotNull()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Token]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Token]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.NewPassword)
                .NotNull()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.ConfirmPassword)
                .NotNull()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.ConfirmPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.ConfirmPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.NewPassword)
                .Equal(req => req.ConfirmPassword)
                .WithMessage(x => $"{_localizer[Constants.MessageKeys.PasswordNotMatch]}");

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Users.Any(x =>
                        x.Email.ToLower() == req.Email.ToLower() && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
            //  .Matches(x => "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$%\\^&\\*]).{8,}$")
        }
    }
}
