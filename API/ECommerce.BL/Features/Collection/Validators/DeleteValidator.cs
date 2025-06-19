using System.Linq;
using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Collections.Validators
{
    public class DeleteCollectionValidator : AbstractValidator<DeleteCollectionRequest>
    {
        private readonly IStringLocalizer<CreateCollectionValidator> _localizer;

        public DeleteCollectionValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateCollectionValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Collections.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Collection]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
