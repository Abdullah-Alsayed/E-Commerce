using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Expenses.Validators
{
    public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateExpenseValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateExpenseValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Reference)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Reference]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Reference]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Amount)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Amount]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Amount]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .LessThan(int.MaxValue)
                .WithMessage(x =>
                    _localizer[Constants.MessageKeys.MaxNumber, int.MaxValue].ToString()
                )
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 1].ToString());

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
                .When(rqe => rqe.FormFile != null)
                .WithMessage(x => _localizer[Constants.MessageKeys.InvalidExtension].ToString())
                .Must(req =>
                {
                    return (req.Length / 1024) > 3000 ? false : true;
                })
                .When(rqe => rqe.FormFile != null)
                .WithMessage(x => _localizer[Constants.MessageKeys.InvalidSize, 3].ToString());
        }
    }
}
