using ConwaysGameOfLife.Api;
using ConwaysGameOfLife.Api.Endpoints;
using ConwaysGameOfLife.Api.Middlewares;
using ConwaysGameOfLife.Core;
using ConwaysGameOfLife.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration)
                .AddInfrastructure(builder.Configuration)
                .AddCore();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.MapBoardEndpoints();

app.Run();