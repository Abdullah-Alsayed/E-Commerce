using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace ECommerce.Core.Helpers
{
    public static class ValidationExtensions
    {
        public static async Task<List<string>> ValidateAsync<T>(
            this IValidator<T> validator,
            T request
        )
        {
            var result = await validator.ValidateAsync(request);
            return result.IsValid
                ? new List<string>()
                : result.Errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}
