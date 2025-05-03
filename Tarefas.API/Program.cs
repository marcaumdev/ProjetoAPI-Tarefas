using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tarefas.API.Context;
using Tarefas.API.Endpoints;
using Tarefas.API.Model;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=tarefas.db";
builder.Services.AddSqlite<TarefasDbContext>(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Tarefas",
        Version = "v1",
        Description = "API do Projeto de Tarefas para aprendizado de desenvolvimento Backend"
    });
}
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Tarefas v1");
    });
}

app.UseHttpsRedirection();

app.MapCategoriasEndpoints();
app.MapTarefasEndpoints();

app.Run();
