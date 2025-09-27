

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicyCors", policy =>
    {
        //cors era para todos los origenes
        policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("MyPolicyCors");
app.MapControllers();
app.Run();
