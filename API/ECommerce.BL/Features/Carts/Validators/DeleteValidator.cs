using System.Linq;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Carts.Validators
{
    public class DeleteCartValidator : AbstractValidator<DeleteCartRequest>
    {
        private readonly IStringLocalizer<CreateCartValidator> _localizer;

        public DeleteCartValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateCartValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Products.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
