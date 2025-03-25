namespace ECommerce.Core.PermissionsClaims
{
    public class ClaimDto
    {
        public string Id { get; set; }
        public string Claim { get; set; }
        public string Module { get; set; }
        public string Operation { get; set; }
        public bool IsChecked { get; set; }
    }
}
