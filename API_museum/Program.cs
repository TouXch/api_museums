using Microsoft.EntityFrameworkCore;
using API_museum.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregando la configuracion de la conexion a la Base de Datos a traves de la connectionstring creada en appsettings.json
builder.Services.AddDbContext<BdMuseumContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

//Eliminando referencias ciclicas para las consultas de listar
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//Agregando los CORS para que nuestra API pueda ser accedida desde cualquier dominio
var misReglasCors = "ReglasCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: misReglasCors, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(misReglasCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
