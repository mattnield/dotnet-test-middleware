namespace MyMiddleware
{
  public class RequestMiddleware
  {
    private readonly RequestDelegate _next;


    public RequestMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env)
    {
      Console.WriteLine($"Running RequestMiddleware in {env.EnvironmentName} environment on {context.Request.Path}.");
      await _next(context);
    }
  }
}