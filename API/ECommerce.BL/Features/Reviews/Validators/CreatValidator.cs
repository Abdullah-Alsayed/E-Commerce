using System.Linq;
using System.Net.Http;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Reviews.Validators
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer _localizer;

        public CreateReviewValidator(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<CreateReviewValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _httpContext = httpContextAccessor;

            _localizer = localizer;

            RuleFor(req => req.Review)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Rate)
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
                .LessThanOrEqualTo(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Rate]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Rate]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.ProductID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Products.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    var userId = _httpContext
                        .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.ID)
                        ?.Value;

                    return !context.Reviews.Any(x =>
                        x.ProductID == req.ProductID
                        && x.CreateBy == userId
                        && x.IsActive
                        && !x.IsDeleted
                    );
                })
                .WithMessage(x => $"{_localizer[Constants.MessageKeys.HasReview]}");
        }
    }
}
