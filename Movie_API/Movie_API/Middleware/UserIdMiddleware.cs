namespace Movie_API.Middleware { 

public class UserIdMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            context.Items["UserId"] = userId;
        }

        await _next(context);
    }
}
}
