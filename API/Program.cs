using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); //using it from Extenions methods
builder.Services.AddIdentityServices(builder.Configuration);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var app = builder.Build();

// Configure the HTTP request pipeline. 
//this is the exception middleware written at the highest level of the middleware section and written inside the ExceptionMiddleware class in InvokeAsync method
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication(); //do you have a valid token

app.UseAuthorization(); //ok ypu hava a valid token now what are u allowed to do 

app.MapControllers();

//seeding dummy data in code 
using var scope = app.Services.CreateScope(); //will give us access to all services we have in program.cs
var services = scope.ServiceProvider;  //saving services
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);

}
catch(Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex,"an error occurred during migration");
}
app.Run();
