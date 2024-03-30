using System.Linq;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleRequest>
    {
        private readonly IStringLocalizer<CreateRoleValidator> _localizer;

        public DeleteRoleValidator(
            RoleManager<Role> roleManager,
            IStringLocalizer<CreateRoleValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return roleManager.Roles.Any(x => x.Id == req.ID.ToString());
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.NotFound]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return roleManager.Roles.Any(x => x.Id == req.ID.ToString() && !x.IsDefault);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Role]} {_localizer[Constants.MessageKeys.IsDefault]}"
                );
        }
    }
}
