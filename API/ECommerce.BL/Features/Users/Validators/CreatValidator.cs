using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.Core.Helpers;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateUserValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateUserValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Email)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .EmailAddress()
                .WithMessage(x => $"{_localizer[Constants.MessageKeys.EmailNotValid]}");

            RuleFor(req => req.PhoneNumber)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(req =>
                {
                    return GeneralHelpers.IsPhoneValid(req);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.Exist]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Users.Any(x =>
                        x.Email.ToLower() == req.Email.ToLower()
                        || x.PhoneNumber == req.PhoneNumber && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
