using System.Linq;
using System.Security.Claims;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

internal class ChangePasswordUserValidator : AbstractValidator<ChangePasswordUserRequest>
{
    private readonly IStringLocalizer _localizer;

    public ChangePasswordUserValidator(
        ApplicationDbContext context,
        IStringLocalizer<ChangePasswordUserValidator> localizer,
        UserManager<User> _userManager,
        IHttpContextAccessor _httpContext
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.OldPassword)
            .NotNull()
            .WithMessage(req =>
                $"{_localizer[Constants.EntityKeys.OldPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(req =>
                $"{_localizer[Constants.EntityKeys.OldPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.NewPassword)
            .NotNull()
            .WithMessage(req =>
                $"{_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(req =>
                $"{_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.NewPassword)
            .Equal(req => req.OldPassword)
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NewPassword]} {_localizer[Constants.MessageKeys.PasswordNotStrong]}"
            );

        //  .Matches(x => "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$%\\^&\\*]).{8,}$")

        RuleFor(x => x)
            .MustAsync(
                async (req, ct) =>
                {
                    var userId = _httpContext
                        .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                        ?.Value;
                    var user = await _userManager
                        .Users.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive && !x.IsDeleted);
                    return await _userManager.CheckPasswordAsync(user, req.OldPassword);
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.OldPassword]} {_localizer[Constants.MessageKeys.NotValid]}"
            );
    }
}
