namespace ECommerce.BLL.Request
{
    public class BaseGridRequest
    {
        public string SearchFor { get; set; } = string.Empty;
        public string SortBy { get; set; } = "ID";
        public string SearchBy { get; set; }
        public bool IsDescending { get; set; } = false;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 50;
    }
}
