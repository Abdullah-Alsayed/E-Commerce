// Ignore Spelling: Validator Validators

using System.Linq;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Products.Validators
{
    public class GetProductItemsValidator : AbstractValidator<GetProductItemsRequest>
    {
        private readonly IStringLocalizer<GetProductItemsValidator> _localizer;

        public GetProductItemsValidator(
            ApplicationDbContext context,
            IStringLocalizer<GetProductItemsValidator> localizer
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
                    return context.Products.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
