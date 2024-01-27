namespace ECommerce.BLL.Response
{
    public class BaseGridResponse<Dto>
        where Dto : class
    {
        public Dto Items { get; set; }
        public int Total { get; set; }
    }
}
