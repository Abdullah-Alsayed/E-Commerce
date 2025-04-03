using ECommerce.DAL.Entity;

namespace ECommerce.BLL.Request
{
    public record BaseGridRequest
    {
        public string SearchFor { get; set; } = string.Empty;
        public string SortBy { get; set; } = nameof(BaseEntity.CreateAt);
        public string SearchBy { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 50;
    }
}
