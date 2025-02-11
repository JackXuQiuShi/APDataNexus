using System.Net;

namespace APWeb.Service.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int? StatusCode { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = "", int? statusCode = null)
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message, StatusCode = statusCode };
        }

        public static ServiceResult<T> FailureResult(string message, int? statusCode = null)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message, StatusCode = statusCode };
        }

        // Alias ErrorMessage to Message
        public string ErrorMessage => !IsSuccess ? Message : null;
    }

}
