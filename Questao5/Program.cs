using MediatR;
using Microsoft.Extensions.Options;
using Questao5.Application;
using Questao5.Domain.Sqlite;
using Questao5.Infrastructure;
using Questao5.Util;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
services.AddControllers();

services.AddMediatR(Assembly.GetExecutingAssembly());

services.AddInfrastructure(configuration);
services.AddApplication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    //c.AddSecurityDefinition("IdempotencyKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //{
    //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    //    Name = "Idempotency-Key",
    //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,

    //    Description = "Chave de idempotência para garantir que requisições duplicadas não serão processadas mais de uma vez."
    //});

    c.OperationFilter<IdempotencyKeyHeaderOperationFilter>();
});

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

app.Services.GetService<IDatabaseBootstrap>()?.Setup();

app.Run();