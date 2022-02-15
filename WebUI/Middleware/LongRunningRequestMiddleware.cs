namespace WebUI.Middleware;

public class LongRunningRequestMiddleware
{
    private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);
    private readonly RequestDelegate _next;

    public LongRunningRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var isElapsed = true;
        var timer = new System.Timers.Timer(_timeout.TotalMilliseconds)
        {
            AutoReset = false
        };

        timer.Elapsed += (_, _) =>
        {
            if (isElapsed)
            {
                context.Abort();
            }
        };

        timer.Start();

        await _next(context);
        isElapsed = false;
    }
}