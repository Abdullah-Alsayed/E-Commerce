using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Categories.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    private readonly IStringLocalizer<UpdateCategoryValidator> _localizer;

    public UpdateCategoryValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateCategoryValidator> localizer
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
                $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
        RuleFor(req => req.NameAR)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Categories.Any(x =>
                        x.NameAR.ToLower() == req.NameAR.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.NameEN)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Categories.Any(x =>
                        x.NameEN.ToLower() == req.NameEN.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.Exist]}"
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
    }
}
