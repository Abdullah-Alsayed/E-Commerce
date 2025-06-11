using System;

namespace ECommerce.DAL.Entity
{
    public class TokenExpired
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }
}
