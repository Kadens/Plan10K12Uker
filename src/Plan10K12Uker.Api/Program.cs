using Microsoft.EntityFrameworkCore;
using Plan10K12Uker.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
    options.UseNpgsql(sp.GetRequiredService<IConfiguration>().GetConnectionString("Postgres"))
        .UseSnakeCaseNamingConvention());
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/api/health");

app.Run();

public partial class Program;
