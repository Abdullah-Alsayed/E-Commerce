using ECommerce.BLL.Response;

namespace ECommerce.BLL.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}

public class BaseResponse<T> : BaseResponse
{
    public T Result { get; set; }
    public int Count { get; set; }
}
