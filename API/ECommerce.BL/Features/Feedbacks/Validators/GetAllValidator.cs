using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Feedbacks.Validators;

public class GetAllFeedbackValidator : AbstractValidator<GetAllFeedbackRequest>
{
    private readonly IStringLocalizer<GetAllFeedbackValidator> _localizer;

    public GetAllFeedbackValidator(IStringLocalizer<GetAllFeedbackValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllFeedbackValidator>(_localizer));
    }
}
