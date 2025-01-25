using System;
using System.Collections.Generic;
using System.Linq;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators
{
    public class UpdateUserClaimsValidator : AbstractValidator<UpdateUserClaimsRequest>
    {
        private readonly IStringLocalizer<UpdateUserClaimsValidator> _localizer;

        public UpdateUserClaimsValidator(
            ApplicationDbContext context,
            IStringLocalizer<UpdateUserClaimsValidator> localizer
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

            RuleFor(req => req.Claims)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Claim]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    var claimsUser = context
                        .UserClaims.Where(User => User.UserId == req.UserID)
                        .Select(x => x.ClaimValue)
                        .ToList();
                    var requestClaims = req.Claims.Distinct().ToList();
                    if (claimsUser.SequenceEqual(requestClaims))
                        return false;
                    else
                        return true;
                })
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Claim]} {_localizer[Constants.MessageKeys.NotChanged]}"
                );

            RuleFor(x => x)
                .Must(req =>
                {
                    var AllPermissions = Permissions.GetAllPermissions();
                    foreach (var claim in req.Claims)
                        if (!AllPermissions.Any(x => x.Claim == claim))
                            return false;

                    return true;
                })
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Claim]} {_localizer[Constants.MessageKeys.NotExist]}"
                );
        }
    }
}
