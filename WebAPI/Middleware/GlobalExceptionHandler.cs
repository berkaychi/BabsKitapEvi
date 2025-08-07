using Microsoft.AspNetCore.Builder;

namespace BabsKitapEvi.WebAPI.Middleware
{
    public static class GlobalExceptionHandler
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}