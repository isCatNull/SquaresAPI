using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebUI.Middleware;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(settings =>
{
    settings.PostProcess = document =>
    {
        document.Info.Title = "Squares API";
        document.Info.Description = "API that brightens the day by finding squares in given list of points.";
    };
});

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(x =>
    x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<ISquareCalculationService, SquareCalculationService>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();

var services = scope.ServiceProvider;
var appDbContext = services.GetRequiredService<AppDbContext>();

await appDbContext.Database.EnsureCreatedAsync();


app.UseOpenApi();
app.UseSwaggerUi3(settings =>
{
    settings.DocExpansion = "list";
    settings.DocumentTitle = "Squares API";
});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<LongRunningRequestMiddleware>();

app.Run();

public partial class Program { }