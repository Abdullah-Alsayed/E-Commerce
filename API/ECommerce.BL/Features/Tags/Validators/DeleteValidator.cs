using System.Linq;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Tags.Validators
{
    public class DeleteTagValidator : AbstractValidator<DeleteTagRequest>
    {
        private readonly IStringLocalizer<CreateTagValidator> _localizer;

        public DeleteTagValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateTagValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Tags.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Tag]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
