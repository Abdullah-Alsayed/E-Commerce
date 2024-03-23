using System.Linq;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Feedbacks.Validators
{
    public class DeleteFeedbackValidator : AbstractValidator<DeleteFeedbackRequest>
    {
        private readonly IStringLocalizer<CreateFeedbackValidator> _localizer;

        public DeleteFeedbackValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateFeedbackValidator> localizer
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
        }
    }
}
