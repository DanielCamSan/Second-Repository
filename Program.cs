using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPoliticaCors", policy =>
    {
        policy.WithOrigins("",
                  "http:127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, ct) =>
    await ctx.HttpContext.Response.WriteAsync("Too many attempts, try it in one minute", ct);
    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 3;                    
        config.Window = TimeSpan.FromMinutes(1);    
        config.QueueLimit = 0;                      
    });
});
*/

builder.Services.AddControllers();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("MiPoliticaCors");
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("default");
app.Run();