using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Brands.Validators;

public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
{
    private readonly IStringLocalizer<UpdateBrandValidator> _localizer;

    public UpdateBrandValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateBrandValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Brands.Any(x => x.Id == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Brand]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
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
            )
            .Must(
                (req, name) =>
                {
                    return !context.Brands.Any(x =>
                        x.NameAR.ToLower() == req.NameAR.ToLower() && x.Id != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.Exist]}"
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
            )
            .Must(
                (req, name) =>
                {
                    return !context.Brands.Any(x =>
                        x.NameEN.ToLower() == req.NameEN.ToLower() && x.Id != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.FormFile)
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
            .When(x => x.FormFile != null)
            .WithMessage(x => _localizer[Constants.MessageKeys.InvalidExtension].ToString())
            .Must(req =>
            {
                return (req.Length / 1024) > 3000 ? false : true;
            })
            .When(x => x.FormFile != null)
            .WithMessage(x => _localizer[Constants.MessageKeys.InvalidSize, 3].ToString());
    }
}
