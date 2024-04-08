using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordRequest>
{
    private readonly IStringLocalizer<ChangeUserPasswordValidator> _localizer;

    public ChangeUserPasswordValidator(
        ApplicationDbContext context,
        IStringLocalizer<ChangeUserPasswordValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.NewPassword)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Users.Any(x =>
                    x.Id == req.ID.ToString() && x.IsActive && !x.IsDeleted
                );
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
