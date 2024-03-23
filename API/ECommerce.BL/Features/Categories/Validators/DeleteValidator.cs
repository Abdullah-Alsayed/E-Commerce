using System.Linq;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Categories.Validators
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryRequest>
    {
        private readonly IStringLocalizer<CreateCategoryValidator> _localizer;

        public DeleteCategoryValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateCategoryValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Categories.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Category]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
