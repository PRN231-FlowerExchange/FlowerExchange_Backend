using Domain.Entities;
using Infrastructure.DateTimes;
using Infrastructure.ExceptionHandlers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDateTimeProvider();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplicationServices();

string connectionString = builder.Configuration.GetConnectionString("FlowerExchangeDB") ?? throw new ArgumentNullException("NUL CONECTION");
builder.Services.AddPersistence(builder.Configuration, connectionString, Assembly.GetExecutingAssembly().GetName().Name);




var app = builder.Build();

//initial data
InitialiserExtensions.InitialiseDatabaseAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
        context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
        _ = (context.Features.Get<IExceptionHandlerFeature>()?.Error);

    });
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
