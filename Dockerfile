# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY MauvestepBackend.sln .

# Copy project files
COPY API/API.csproj API/
COPY Database/Database.csproj Database/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Publish API project
RUN dotnet publish API/API.csproj -c Release -o /app/publish


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Render uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT}

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]