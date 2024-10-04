using Domain.Entities;
using Domain.FirebaseStorage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Infrastructure.DateTimes;
using Infrastructure.ExceptionHandlers;
using Infrastructure.FirebaseStorage;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
