using System;

namespace ECommerce.Core.Services.User;

public interface IUserContext
{
    (bool Exist, Guid Value) UserId { get; }
    public (bool Exist, string Value) UserName { get; }
    (bool Exist, string Value) Language { get; }
}
