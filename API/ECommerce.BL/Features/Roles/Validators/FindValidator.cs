using System.Linq;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators;

public class FindRoleValidator : AbstractValidator<FindRoleRequest>
{
    private readonly IStringLocalizer<FindRoleValidator> _localizer;

    public FindRoleValidator(
        RoleManager<Role> roleManager,
        IStringLocalizer<FindRoleValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return roleManager.Roles.AsNoTracking().Any(x => x.Id == req.ID);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
