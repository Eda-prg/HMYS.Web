using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using HMYS.Core;


namespace HMYS.Web.Middlewares

{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;
     
       
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "Sistem şu an meşgul veya bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.",
                Detail = exception.Message // Loglama için burayı kullanacağız
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
    

