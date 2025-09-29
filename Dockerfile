# Machine Management API Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/Backend/MachineManagement.API/MachineManagement.API.csproj", "src/Backend/MachineManagement.API/"]
COPY ["src/Backend/MachineManagement.Infrastructure/MachineManagement.Infrastructure.csproj", "src/Backend/MachineManagement.Infrastructure/"]
COPY ["src/Backend/MachineManagement.Core/MachineManagement.Core.csproj", "src/Backend/MachineManagement.Core/"]

RUN dotnet restore "src/Backend/MachineManagement.API/MachineManagement.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Backend/MachineManagement.API"
RUN dotnet build "MachineManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MachineManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__DefaultConnection="Server=mysql-db;Database=MachineManagementDB;Uid=root;Pwd=production_password;"

ENTRYPOINT ["dotnet", "MachineManagement.API.dll"]