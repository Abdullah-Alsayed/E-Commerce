using System;
using System.Linq;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators;

public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleRequest>
{
    private readonly IStringLocalizer<UpdateUserRoleValidator> _localizer;

    public UpdateUserRoleValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateUserRoleValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.UserID)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(ID =>
            {
                return context.Users.AsNoTracking().Any(x => x.Id == ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.NotExist]}"
            );

        RuleForEach(req => req.RoleIDs)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.RoleIDs)
            .Must(Ids =>
            {
                return context.Roles.AsNoTracking().Any(x => Ids.Contains(x.Id) && !x.IsDeleted);
            })
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.NotExist]}"
            );
    }
}
