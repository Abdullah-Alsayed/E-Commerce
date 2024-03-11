using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Reviews.Validators;

public class GetAllReviewValidator : AbstractValidator<GetAllReviewRequest>
{
    private readonly IStringLocalizer<GetAllReviewValidator> _localizer;

    public GetAllReviewValidator(IStringLocalizer<GetAllReviewValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllReviewValidator>(_localizer));
    }
}
