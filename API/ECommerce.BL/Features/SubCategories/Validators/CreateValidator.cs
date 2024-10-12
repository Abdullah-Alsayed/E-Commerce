using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.SubCategorys.Validators
{
    public class CreateSubCategoryValidator : AbstractValidator<CreateSubCategoryRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateSubCategoryValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateSubCategoryValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.NameAR)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.NameEN)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Categories.Any(x =>
                        x.ID == req.CategoryID && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req.FormFile)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(path =>
                {
                    var allowedExtensions = Enum.GetNames(typeof(PhotoExtensions)).ToList();
                    var extension = Path.GetExtension(path.FileName.ToLower());
                    if (string.IsNullOrEmpty(extension))
                        return false;

                    extension = extension.Remove(extension.LastIndexOf('.'), 1);
                    if (!allowedExtensions.Contains(extension))
                        return false;

                    return true;
                })
                .WithMessage(x => _localizer[Constants.MessageKeys.InvalidExtension].ToString())
                .Must(req =>
                {
                    return (req.Length / 1024) > 3000 ? false : true;
                })
                .WithMessage(x => _localizer[Constants.MessageKeys.InvalidSize, 3].ToString());

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.SubCategories.Any(x =>
                        (x.NameEN == req.NameEN || x.NameAR == req.NameAR)
                        && x.IsActive
                        && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.SubCategory]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
