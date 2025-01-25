// Ignore Spelling: Validator

using System.Linq;
using Azure.Core;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    private readonly IStringLocalizer<LoginValidator> _localizer;

    public LoginValidator(ApplicationDbContext context, IStringLocalizer<LoginValidator> localizer)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.UserName)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.UserName]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.UserName]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Users.Any(x =>
                    x.UserName == req.UserName.ToLower()
                    || x.Email == req.UserName.ToLower()
                    || x.PhoneNumber == req.UserName && x.IsActive && !x.IsDeleted
                );
            })
            .WithMessage(x => $" {_localizer[Constants.MessageKeys.LoginFiled]}");
    }
}
