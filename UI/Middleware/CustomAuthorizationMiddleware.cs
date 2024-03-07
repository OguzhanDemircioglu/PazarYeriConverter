namespace UI.Middleware
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Session.GetString("Authorization");

            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", token);

            await _next(context);
        }
    }
}
