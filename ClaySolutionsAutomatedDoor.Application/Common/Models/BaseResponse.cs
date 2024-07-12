using Newtonsoft.Json;

namespace ClaySolutionsAutomatedDoor.Application.Common.Models
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
        public int StatusCode { get; set; }

        public BaseResponse()
        {
        }

        public BaseResponse(bool status, string message, int statusCode)
        {
            Status = status;
            Message = message;
            StatusCode = statusCode;
        }

        public BaseResponse(bool status, T data, int statusCode)
        {
            Status = status;
            Data = data;
            StatusCode = statusCode;
        }

        public BaseResponse(bool status, string message, T data, int statusCode)
        {
            Status = status;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }



        public static BaseResponse<T> FailedResponse(string errorMessage, int statusCode)
        {
            return new BaseResponse<T>
            {
                Status = false,
                Message = errorMessage,
                StatusCode = statusCode
            };
        }
        public static BaseResponse<T> PassedResponse(string successMessage, T data, int statusCode = 200)
        {
            return new BaseResponse<T>
            {
                Status = true,
                Message = successMessage,
                Data = data,
                StatusCode = statusCode
            };
        }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }

    public class BaseResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }
        public int StatusCode { get; set; }
        public BaseResponse()
        {
        }

        public BaseResponse(bool status, string message, int statusCode)
        {
            Status = status;
            Message = message;
            StatusCode = statusCode;
        }

        public BaseResponse(bool status, int statusCode)
        {
            Status = status;
            StatusCode = statusCode;
        }

        public static BaseResponse FailedResponse(string message, int statusCode)
        {
            return new BaseResponse(status: false, message, statusCode);
        }

        public static BaseResponse PassedResponse(string message, int statusCode)
        {
            return new BaseResponse(status: true, message, statusCode);
        }
    }
}
