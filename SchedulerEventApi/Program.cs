using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SchedulerEventApi.Extensions;
using SchedulerEventRepositories.DbContext;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ContextApp>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")), ServiceLifetime.Transient);
builder.Services.RegisterServices();
builder.Services.ConfigureCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(builder.Configuration.GetSection("AuthenticationToken:Token").Value);

var app = builder.Build();

//Set Culture by Default
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.ConfigureExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   

}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
