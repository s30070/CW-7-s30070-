// Na początku każdego pliku
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Services

{
    public class ServiceResult
    {
        public bool Success { get; }
        public string ErrorMessage { get; }
        public int StatusCode { get; }

        protected ServiceResult(bool success, string errorMessage, int statusCode)
        {
            Success = success;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static ServiceResult Ok() => new ServiceResult(true, null, 200);
        public static ServiceResult<T> Ok<T>(T value) => new ServiceResult<T>(value, true, null, 200);
        public static ServiceResult Fail(string message, int statusCode) => new ServiceResult(false, message, statusCode);
        public static ServiceResult<T> Fail<T>(string message, int statusCode) => new ServiceResult<T>(default, false, message, statusCode);

        public IActionResult ToActionResult() => 
            Success ? new OkResult() : new ObjectResult(ErrorMessage) { StatusCode = StatusCode };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Value { get; }

        public ServiceResult(T value, bool success, string errorMessage, int statusCode) 
            : base(success, errorMessage, statusCode)
        {
            Value = value;
        }

        public new IActionResult ToActionResult() => 
            Success ? new OkObjectResult(Value) : new ObjectResult(ErrorMessage) { StatusCode = StatusCode };
    }
}