namespace ECommerce.BLL.Features.ContactUses.Requests
{
    public record CreateContactUsRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
