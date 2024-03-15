using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sliders.Validators
{
    public class CreateSliderValidator : AbstractValidator<CreateSliderRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateSliderValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateSliderValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.TitleAR)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.TitleAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.TitleAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.TitleEN)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.TitleEN]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.TitleEN]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Description)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.FormFile)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
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
                    return !context.Sliders.Any(x =>
                        x.TitleEN == req.TitleEN
                        || x.TitleAR == req.TitleAR && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Slider]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
