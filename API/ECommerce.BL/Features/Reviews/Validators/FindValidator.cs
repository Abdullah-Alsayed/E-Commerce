using System.Linq;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Reviews.Validators;

public class FindReviewValidator : AbstractValidator<FindReviewRequest>
{
    private readonly IStringLocalizer<FindReviewValidator> _localizer;

    public FindReviewValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindReviewValidator> localizer
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
                return context.Reviews.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Review]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
