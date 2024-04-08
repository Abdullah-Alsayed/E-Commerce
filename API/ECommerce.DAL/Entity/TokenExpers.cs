using System;

namespace ECommerce.DAL.Entity
{
    public class TokenExperts
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string UserID { get; set; }
        public string Token { get; set; }
    }
}
