using System.Linq;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Feedbacks.Validators;

public class FindFeedbackValidator : AbstractValidator<FindFeedbackRequest>
{
    private readonly IStringLocalizer<FindFeedbackValidator> _localizer;

    public FindFeedbackValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindFeedbackValidator> localizer
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
                return context.Feedbacks.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Feedback]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
