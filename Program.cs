using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPoliticaCors", policy =>
    {
        //El cors era para todos no solo para algunas ips
        policy.WithOrigins("https://localhost:7047",
                  "http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



builder.Services.AddControllers();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("MiPoliticaCors");
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("default");
app.Run();