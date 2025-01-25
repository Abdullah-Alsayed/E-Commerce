using System.Linq;
using System.Security.Claims;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

public class ChangePasswordUserValidator : AbstractValidator<ChangePasswordUserRequest>
{
    private readonly IStringLocalizer _localizer;

    public ChangePasswordUserValidator(
        ApplicationDbContext context,
        IStringLocalizer<ChangePasswordUserValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.OldPassword)
            .NotNull()
            .WithMessage(req =>
                $"{_localizer[Constants.EntityKeys.OldPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(req =>
                $"{_localizer[Constants.EntityKeys.OldPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
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

        //  .Matches(x => "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$%\\^&\\*]).{8,}$")
    }
}
