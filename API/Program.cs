using API.Middleware;
using Core.Interfaces;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>( opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
//builder.Services.AddScoped<IProductRepository , ProductRepository>();
builder.Services.AddScoped(typeof(IGenericrepository<>),typeof(GenericRepository<>));
builder.Services.AddCors();


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200"));
app.MapControllers();


try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await storecontextseed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
