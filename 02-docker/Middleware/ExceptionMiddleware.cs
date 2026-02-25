using System.Net;
using System.Text.Json;

namespace server.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // עבור לחלק הבא של ה-Pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה בלתי צפויה בשרת"); // רושם את השגיאה
                await HandleExceptionAsync(context, ex); // טיפול בשגיאה
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                message = "שגיאה בשרת",
                detail = exception.Message,
                statusCode = context.Response.StatusCode,
                timestamp = DateTime.UtcNow
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
