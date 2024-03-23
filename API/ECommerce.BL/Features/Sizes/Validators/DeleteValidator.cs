using System.Linq;
using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sizes.Validators
{
    public class DeleteSizeValidator : AbstractValidator<DeleteSizeRequest>
    {
        private readonly IStringLocalizer<CreateSizeValidator> _localizer;

        public DeleteSizeValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateSizeValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Sizes.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Size]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
