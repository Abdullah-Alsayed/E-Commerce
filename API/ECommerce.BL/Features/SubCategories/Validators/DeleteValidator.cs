using System.Linq;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.SubCategorys.Validators
{
    public class DeleteSubCategoryValidator : AbstractValidator<DeleteSubCategoryRequest>
    {
        private readonly IStringLocalizer<CreateSubCategoryValidator> _localizer;

        public DeleteSubCategoryValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateSubCategoryValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.SubCategories.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.SubCategory]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
