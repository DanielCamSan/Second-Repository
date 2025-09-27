var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    //configuracion cors era para todos los origines
    options.AddPolicy("MiPoliticaCors", policy =>
    {
        policy.WithOrigins("https://localhost:7162", "http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("MiPoliticaCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
