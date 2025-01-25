using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators;

public class AddUserToRoleValidator : AbstractValidator<AddUserToRoleRequest>
{
    private readonly IStringLocalizer<AddUserToRoleValidator> _localizer;

    public AddUserToRoleValidator(
        ApplicationDbContext context,
        IStringLocalizer<AddUserToRoleValidator> localizer
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

        RuleFor(req => req)
            .Must(req =>
            {
                return !context
                    .UserRoles.AsNoTracking()
                    .All(x => x.UserId == req.UserID && req.RoleIDs.Contains(x.RoleId));
            })
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.UserInThisRole]}");
    }
}
