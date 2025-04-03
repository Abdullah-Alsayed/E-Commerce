using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateProductValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateProductValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Title)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Title]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Title]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Description)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(3)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Price)
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 1].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Price]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Price]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.BrandID)
                .Must(ID =>
                {
                    return context.Brands.Any(x => x.Id == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Brand]} {_localizer[Constants.MessageKeys.NotExist]}"
                )
                .When(req => req.BrandID.HasValue);

            RuleFor(req => req.UnitID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Unit]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Unit]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Units.Any(x => x.Id == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Unit]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req.CategoryID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Categories.Any(x => x.Id == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req.SubCategoryID)
                .Must(ID =>
                {
                    return context.SubCategories.Any(x => x.Id == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.SubCategory]} {_localizer[Constants.MessageKeys.NotExist]}"
                )
                .When(req => req.SubCategoryID.HasValue);

            RuleFor(req => req.FormFiles)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleForEach(req => req.FormFiles)
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
                    return req.Length / 1024 > 3000 ? false : true;
                })
                .WithMessage(x => _localizer[Constants.MessageKeys.InvalidSize, 3].ToString());

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Products.Any(x =>
                        x.Title == req.Title || x.Title == req.Title && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
