using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.SubCategories.Requests
{
    public record GetAllSubCategoryRequest : BaseGridRequest
    {
        public Guid? CategoryId { get; set; }
    }
}
