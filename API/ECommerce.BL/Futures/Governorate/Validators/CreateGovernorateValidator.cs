using ECommerce.BLL.Futures.Governorate.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using System.Linq;

namespace ECommerce.BLL.Futures.Governorate.Validators
{
    public class CreateGovernorateValidator : AbstractValidator<CreateGovernorateRequest>
    {
        public CreateGovernorateValidator(Applicationdbcontext context)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(req => req.NameAR)
                .MaximumLength(100)
                .WithMessage(x => "MaximumLength")
                .MinimumLength(5)
                .WithMessage("MinimumLength")
                .NotEmpty()
                .WithMessage("NotEmpty")
                .NotNull()
                .WithMessage("NotNull");

            RuleFor(req => req.NameEN)
                .MaximumLength(100)
                .WithMessage(x => "MaximumLength")
                .MinimumLength(5)
                .WithMessage("MinimumLength")
                .NotEmpty()
                .WithMessage("NotEmpty")
                .NotNull()
                .WithMessage("NotNull");

            RuleFor(req => req.Tax)
                .NotNull()
                .WithMessage("NotNull")
                .NotEmpty()
                .WithMessage("NotEmpty")
                .LessThan(int.MaxValue);

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Governorates.Any(
                        x =>
                            x.NameEN == req.NameEN
                            || x.NameAR == req.NameAR && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x => Constants.Errors.NotFound);
        }
    }
}
