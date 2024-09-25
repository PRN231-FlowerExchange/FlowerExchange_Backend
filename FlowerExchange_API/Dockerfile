#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FlowerExchange_API/Presentation.csproj", "FlowerExchange_API/"]
COPY ["CrossCuttingConcerns/CrossCuttingConcerns.csproj", "CrossCuttingConcerns/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["FlowerExchange_Repositories/Persistence.csproj", "FlowerExchange_Repositories/"]
COPY ["FlowerExchange_Services/Application.csproj", "FlowerExchange_Services/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "./FlowerExchange_API/Presentation.csproj"
COPY . .
WORKDIR "/src/FlowerExchange_API"
RUN dotnet build "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]