using System.Linq;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vendors.Validators
{
    public class DeleteVendorValidator : AbstractValidator<DeleteVendorRequest>
    {
        private readonly IStringLocalizer<CreateVendorValidator> _localizer;

        public DeleteVendorValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateVendorValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Vendors.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Vendor]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
