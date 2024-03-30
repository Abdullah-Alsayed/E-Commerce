using System.Linq;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators;

public class UpdateRoleValidator : AbstractValidator<UpdateRoleRequest>
{
    private readonly IStringLocalizer<UpdateRoleValidator> _localizer;

    public UpdateRoleValidator(
        RoleManager<Role> roleManager,
        IStringLocalizer<UpdateRoleValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return roleManager
                    .Roles.AsNoTracking()
                    .Any(x => x.Id == req.ID.ToString() && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.NotFound]}"
            );

        RuleFor(req => req.Name)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !roleManager
                        .Roles.AsNoTracking()
                        .Any(x =>
                            x.Name.ToLower() == req.Name.ToLower() && x.Id != req.ID.ToString()
                        );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Description)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );
    }
}
