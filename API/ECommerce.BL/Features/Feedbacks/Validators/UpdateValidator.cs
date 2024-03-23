using System.Linq;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Feedbacks.Validators;

public class UpdateFeedbackValidator : AbstractValidator<UpdateFeedbackRequest>
{
    private readonly IStringLocalizer<UpdateFeedbackValidator> _localizer;

    public UpdateFeedbackValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateFeedbackValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Feedbacks.Any(x => x.ID == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Feedback]} {_localizer[Constants.MessageKeys.NotFound]}"
            );

        RuleFor(req => req.Comment)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Comment]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Comment]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.Rating)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
            .LessThanOrEqualTo(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Rating]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Rating]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );
    }
}
