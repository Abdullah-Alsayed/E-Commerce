using System.Linq;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators
{
    public class UpdateRoleClaimsValidator : AbstractValidator<UpdateRoleClaimsRequest>
    {
        private readonly IStringLocalizer<UpdateRoleClaimsValidator> _localizer;

        public UpdateRoleClaimsValidator(
            ApplicationDbContext context,
            IStringLocalizer<UpdateRoleClaimsValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.RoleID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Roles.AsNoTracking().Any(x => x.Id == ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req.Claims)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Claim]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    var claimsRole = context
                        .RoleClaims.Where(role => role.RoleId == req.RoleID)
                        .Select(x => x.ClaimValue)
                        .ToList();
                    var requestClaims = req.Claims.Distinct().ToList();
                    if (claimsRole.SequenceEqual(requestClaims))
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
                    var _roleMaster = context.Roles.FirstOrDefault(x => x.IsMaster);
                    var claimsMasterRole = context
                        .RoleClaims.Where(role => role.RoleId == _roleMaster.Id)
                        .ToList();

                    foreach (var claim in req.Claims)
                        if (!claimsMasterRole.Any(x => x.ClaimValue == claim))
                            return false;

                    return true;
                })
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Claim]} {_localizer[Constants.MessageKeys.NotExist]}"
                );
        }
    }
}
