using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiCrud.Models;
using WebApiCrud.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Register your services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IContactService, ContactService>();
// Add other services as needed...

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name V1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
