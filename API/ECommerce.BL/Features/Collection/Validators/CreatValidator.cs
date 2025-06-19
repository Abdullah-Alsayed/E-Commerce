using System.Linq;
using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.Core;
using ECommerce.Core.Helpers;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Collections.Validators
{
    public class CreateCollectionValidator : AbstractValidator<CreateCollectionRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateCollectionValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateCollectionValidator> localizer
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

            RuleFor(req => req.FormFile)
                .Must(path =>
                {
                    return FileHelper.ExtensionsCheck(path);
                })
                .When(x => x.FormFile != null)
                .WithMessage(x => _localizer[Constants.MessageKeys.InvalidExtension].ToString())
                .Must(path =>
                {
                    return FileHelper.SizeCheck(path);
                })
                .When(x => x.FormFile != null)
                .WithMessage(x =>
                    _localizer[Constants.MessageKeys.InvalidSize, Constants.FileSize].ToString()
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Collections.Any(x =>
                        (x.NameEN == req.NameEN || x.NameAR == req.NameAR)
                        && x.IsActive
                        && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Collection]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
