using System.Linq;
using ECommerce.BLL.Features.ContactUss.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.ContactUss.Validators
{
    public class DeleteContactUsValidator : AbstractValidator<DeleteContactUsRequest>
    {
        private readonly IStringLocalizer<DeleteContactUsValidator> _localizer;

        public DeleteContactUsValidator(
            ApplicationDbContext context,
            IStringLocalizer<DeleteContactUsValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.ContactUs.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.ContactUs]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
