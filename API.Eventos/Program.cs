using API.Eventos.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
builder.Services.AddEndpointsApiExplorer();
// Customizando nossas informações

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "API de Eventos",
            Description = "Sistema de Gerenciamento de Eventos e Inscrições",
            Contact = new OpenApiContact
            {
                Name = "Desenvolvido por George",
                Url = new Uri("https://localhost:7141/swagger/index.html")
            }
        });

    // Pegamos o nome no nosso arquivo Assembly e armazenamos na variável
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    // Pega o diretório base da aplicação e junta com o nome do arquivo que acabou de criar
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
