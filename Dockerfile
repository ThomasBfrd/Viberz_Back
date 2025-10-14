# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Viberz.API/Viberz.API.csproj", "Viberz.API/"]
COPY ["Viberz.Application/Viberz.Application.csproj", "Viberz.Application/"]
COPY ["Viberz.Domain/Viberz.Domain.csproj", "Viberz.Domain/"]
COPY ["Viberz.Infrastructure/Viberz.Infrastructure.csproj", "Viberz.Infrastructure/"]
RUN dotnet restore "Viberz.API/Viberz.API.csproj"

# Copy the rest of the code
COPY . .

# Build the application
RUN dotnet build "Viberz.API/Viberz.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Viberz.API/Viberz.API.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Viberz.API.dll"]