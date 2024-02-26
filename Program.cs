//Here; we declare our Application Builder:
using HotelListingAPI.Configurations;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using HotelListingAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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

//Setting up my Identity Core for Security Management:
builder.Services.AddIdentityCore<ApiUser>()  //Adding Facility to Validate User-Type using AppUser which EXTENDS the default Identity User Model
    .AddRoles<IdentityRole>() //Adding Facility to Validate User-Role using the default Identity UserRole Model
    .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingAPI")
    .AddEntityFrameworkStores<HotelListingDbContext>() //Deffining the Database to store your Project's Identity Data.
    .AddDefaultTokenProviders();


//Configuring my Logger using serilog: ctx === builder context (acts like our builder); lc === loger configuration that was set inside the appsetting.json
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//Registering our REPOSITORIES" The below two ways are equally appropriate for adding/Registering Service
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(ICountriesRepository), typeof(CountriesRepository));
builder.Services.AddScoped(typeof(IHotelRepository), typeof(HotelsRepository));
builder.Services.AddScoped<IAuthManager, AuthManager>();




//Adding Our Authentication Service:
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //This cascades the string "Bearer"
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer  = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew   = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };

});




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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


//Here we run our Application:
app.Run();
