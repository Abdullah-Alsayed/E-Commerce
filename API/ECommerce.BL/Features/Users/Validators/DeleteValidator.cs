using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
    {
        private readonly IStringLocalizer<CreateUserValidator> _localizer;

        public DeleteUserValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateUserValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Users.Any(x => x.Id == req.ID.ToString() && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
