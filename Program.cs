using System.Text.Json;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

var basePaths = new List<string>() { "/preview", "/sitemap.xml", "/robots.txt", "/favicon.ico", "/kontent", "/lingo24", "/api", "/webhooks", "/css", "/fonts", "/img", "/js", "/CantoCustomElement", "/searchmanagement" };
app.UseWhen(ctx => ctx.Request.Method == "GET" &&  !basePaths.Any(basePath => ctx.Request.Path.StartsWithSegments(basePath, StringComparison.OrdinalIgnoreCase)), appBuilder =>
{
  appBuilder.UseMiddleware<MyMiddleware.RequestMiddleware>();
});

app.Use(async (HttpContext ctx, Func<Task> _) =>
{
  var data = new Dictionary<string, string>();
  data.Add("Path", ctx.Request.Path);
  data.Add("Date", DateTime.Now.ToString());
  data.Add("Method", ctx.Request.Method);
  data.Add("QueryString", ctx.Request.QueryString.ToString());
  data.Add("Scheme", ctx.Request.Scheme);
  data.Add("Host", ctx.Request.Host.ToString());

  var message = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
  ctx.Response.Headers.ContentType = "application/json";
  await ctx.Response.WriteAsync(message);
});

app.Run();
