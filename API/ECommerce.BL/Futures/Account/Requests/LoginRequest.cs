﻿namespace ECommerce.BLL.Futures.Account.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
