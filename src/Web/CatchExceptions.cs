

namespace Reim.Web;

public class CatchExceptions(ILogger<CatchExceptions> log) : IMiddleware {

    public Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            return next(context);
        } catch (Exception ex) {
            log.LogError(ex, "Something went wrong");
            context.Response.StatusCode = 500;
            context.Response.WriteAsync("Something went wrong");
            return Task.CompletedTask;
        }
    }
}
