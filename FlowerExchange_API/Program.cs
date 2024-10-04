using Domain.Constants.Enums;
using Domain.Entities;
using Domain.FirebaseStorage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Infrastructure.DateTimes;
using Infrastructure.EmailProvider.Gmail;
using Infrastructure.ExceptionHandlers;
using Infrastructure.FirebaseStorage;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Infrastructure.Security.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Persistence;
using Presentation.OptionsSetup;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
         Url = "https://flowerexchange.azurewebsites.net",
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



builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

string connectionString = builder.Configuration.GetConnectionString("FlowerExchangeDB") ?? throw new ArgumentNullException("NUL CONECTION");
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, connectionString, Assembly.GetExecutingAssembly().GetName().Name);
builder.Services.AddInfrastructureServices();

//// Retrieve Firebase credentials from the environment variable
//string credentialsPath = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
//if (string.IsNullOrEmpty(credentialsPath))
//{
//    throw new Exception("Google Application Credentials not found in environment variables.");
//}

// Step 1: Retrieve the Firebase credentials JSON content from appsettings.Development.json
var firebaseConfigSection = builder.Configuration.GetSection("FirebaseConfig");
var credentialsJson = firebaseConfigSection.GetSection("CredentialsJson").Get<Dictionary<string, object>>();

// Step 2: Convert the credentials to a JSON string
var credentialsJsonString = JsonConvert.SerializeObject(credentialsJson);

// Step 3: Write the JSON string to a temporary file
var tempFilePath = Path.GetTempFileName(); // Creates a unique temporary file
File.WriteAllText(tempFilePath, credentialsJsonString);

// Set the environment variable for Firebase initialization
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempFilePath);

// Register the Firebase storage service
builder.Services.AddSingleton<IFirebaseStorageService>(s => new FirebaseStorageService(StorageClient.Create()));
builder.Services.ConfigureOptions<JwtConfigOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.ConfigureOptions<EmailOptionsSetup>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

var app = builder.Build();

//initial data
InitialiserExtensions.InitialiseDatabaseAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flower Exchange API V1");
    });
}

app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
        var exception = (context.Features.Get<IExceptionHandlerFeature>()?.Error);
    });
});
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapIdentityApi<User>();

app.Run();
