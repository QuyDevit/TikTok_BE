using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Application.Common
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty; 
        public ServiceResponse() { }

        public ServiceResponse(T data, string message = "")
        {
            Success = true;
            Message = message;
            Data = data;
        }
        public static ServiceResponse<T> Fail(string message)
        {
            return new ServiceResponse<T> { Success = false, Message = message };
        }

        public static ServiceResponse<T> Ok(T data, string message = "")
        {
            return new ServiceResponse<T> { Data = data, Success = true, Message = message };
        }
    }
}
