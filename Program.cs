//Here; we declare our Application Builder:
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Then, we Add services that will be necessary to run our Program to the Dependency-Injection (Services) Container:

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


//Configuring my Logger using serilog: ctx === builder context (acts like our builder); lc === loger configuration that was set inside the appsetting.json
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));



//This is where we Build our Application
var app = builder.Build();



//This is where we configure our Middlewares:

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll"); //This is where we used the CorsPolicy service we defined earlier.

app.UseAuthorization();

app.MapControllers();


//Here we run our Application:
app.Run();
