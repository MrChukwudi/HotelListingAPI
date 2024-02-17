//Here; we declare our Application Builder:
using HotelListingAPI.Configurations;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using HotelListingAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Then, we Add services that will be necessary to run our Program to the Dependency-Injection (Services) Container:


//Setting Up the DB and ConnectionString:
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConectionString"); //Configuration simply points to the appsetting.json file. 
//Exevuting the Databse Connection with the above Connection string, and the DbContext file:
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuring my CORS Policy:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", //We defined A Policy for our Cors according to our need: for 3rd party access.
        b => b.AllowAnyHeader()
              .AllowAnyOrigin()
              .AllowAnyMethod()
              );
});

//Registering/Building/Adding my AutoMapper service to the application DI build process:
builder.Services.AddAutoMapper(typeof(MapperConfig));


//Configuring my Logger using serilog: ctx === builder context (acts like our builder); lc === loger configuration that was set inside the appsetting.json
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//Registering our REPOSITORIES"
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(ICountriesRepository), typeof(CountriesRepository));

//This is where we Build our Application
var app = builder.Build();



//This is where we configure our Middlewares:

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); //For Logging our requests from clients


app.UseHttpsRedirection();

app.UseCors("AllowAll"); //This is where we used the CorsPolicy service we defined earlier.

app.UseAuthorization();

app.MapControllers();


//Here we run our Application:
app.Run();
