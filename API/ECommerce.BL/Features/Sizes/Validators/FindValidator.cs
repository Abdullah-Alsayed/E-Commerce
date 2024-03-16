using System.Linq;
using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sizes.Validators;

public class FindSizeValidator : AbstractValidator<FindSizeRequest>
{
    private readonly IStringLocalizer<FindSizeValidator> _localizer;

    public FindSizeValidator(
        Applicationdbcontext context,
        IStringLocalizer<FindSizeValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x => Constants.Errors.Register)
            .NotNull()
            .WithMessage(x => Constants.Errors.Register);

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Sizes.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Size]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
