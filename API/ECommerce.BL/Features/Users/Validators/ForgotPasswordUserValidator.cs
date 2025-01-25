using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

public class ForgotPasswordUserValidator : AbstractValidator<ForgotPasswordUserRequest>
{
    private readonly IStringLocalizer _localizer;

    public ForgotPasswordUserValidator(
        ApplicationDbContext context,
        IStringLocalizer<ForgotPasswordUserValidator> localizer
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
    }
}
