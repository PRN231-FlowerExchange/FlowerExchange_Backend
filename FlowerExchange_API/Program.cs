﻿using Application;
using Domain.Cloudinary;
using Domain.Cloudinary.Models;
using Domain.Entities;
using Domain.FirebaseStorage;
using Domain.Payment;
using Domain.Payment.Models;
using Google.Cloud.Storage.V1;
using Infrastructure.Cloudinary;
using Infrastructure.ExceptionHandlers;
using Infrastructure.FirebaseStorage;
using Infrastructure.Payment;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Persistence;
using Presentation;
using Presentation.OptionsSetup;
using System.Configuration;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Allow Cors Origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Đảm bảo điều này cho phép cookie/tokens
    });
    //options.AddPolicy("AllowAnyOrigin",
    //    builder =>
    //    {
    //        builder.AllowAnyOrigin()
    //              .AllowAnyHeader()
    //              .AllowAnyMethod();
    //    });
});


//Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: 'Bearer 12345abcdef'",
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    // Define multiple server URLs for Swagger
    c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
    {
        Url = "https://flowerexchange.azurewebsites.net/",
        Description = "Production Server (Azure)"
    });

    c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
    {
        Url = "https://localhost:7246",
        Description = "Local Development Server (HTTPS)"
    });

    c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
    {
        Url = "http://localhost:5223",
        Description = "Local Development Server (HTTP)"
    });
});
//Add Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//Add Other File Configuration
string connectionString = builder.Configuration.GetConnectionString("FlowerExchangeDB") ?? throw new ArgumentNullException("NUL CONECTION");
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, connectionString, Assembly.GetExecutingAssembly().GetName().Name);
builder.Services.AddInfrastructureServices(builder.Configuration);

// VNPAY service
builder.Services.AddScoped<IVNPAYService, VNPAYService>();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

// Momo service
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

// Add Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.ConfigureOptions<JwtConfigOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.ConfigureOptions<EmailOptionsSetup>();

var app = builder.Build();

//Initialize data
InitialiserExtensions.InitialiseDatabaseAsync(app);

//if(app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flower Exchange API V1");
});
//}

app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
        var exception = (context.Features.Get<IExceptionHandlerFeature>()?.Error);
    });
});

app.UseRouting();

//app.UseCors("AllowAnyOrigin");

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();

//app.MapIdentityApi<User>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub"); // SignalR Hub endpoint
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();






