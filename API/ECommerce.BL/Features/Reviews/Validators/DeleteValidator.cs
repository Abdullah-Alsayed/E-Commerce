using System.Linq;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Reviews.Validators
{
    public class DeleteReviewValidator : AbstractValidator<DeleteReviewRequest>
    {
        private readonly IStringLocalizer<CreateReviewValidator> _localizer;

        public DeleteReviewValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateReviewValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Reviews.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Review]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
