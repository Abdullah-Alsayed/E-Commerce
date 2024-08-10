using System.Linq;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Feedbacks.Validators
{
    public class CreateFeedbackValidator : AbstractValidator<CreateFeedbackRequest>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer _localizer;

        public CreateFeedbackValidator(
            ApplicationDbContext context,
            IHttpContextAccessor httpContext,
            IStringLocalizer<CreateFeedbackValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;
            _httpContext = httpContext;

            RuleFor(req => req.Comment)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Comment]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Comment]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Rating)
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
                .LessThanOrEqualTo(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Rating]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Rating]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    var userId = _httpContext
                        .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.ID)
                        ?.Value;

                    return !context.Reviews.Any(x =>
                        x.CreateBy == userId && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x => $"{_localizer[Constants.MessageKeys.HasReview]}");
        }
    }
}
