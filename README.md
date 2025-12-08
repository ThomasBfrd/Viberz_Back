# 📌 Viberz (backend)

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=fff)](#)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=fff)](#)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=fff)](#)
[![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=fff)](#)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-512BD4?logo=.net&logoColor=fff)](#)
[![JWT](https://img.shields.io/badge/JWT-000000?logo=json-web-tokens&logoColor=fff)](#)
[![Spotify API](https://img.shields.io/badge/Spotify_API-1ED760?logo=spotify&logoColor=white)](#)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A REST API for Viberz, a web application dedicated to exploring EDM subgenres.
This API handles authentication, Spotify integration, user progression, and game logic.

## ✨ Feature Details

### 🎮 Game Logic
- **Guess Genre**: Identify EDM subgenres
- **Guess Song**: Identify tracks by genre
- Points and XP system
- Game history tracking

### 👤 Gestion Utilisateurs
- User profiles
- XP-based progression and ranks
- Top artists and favorite genres via Spotify API
- XP history tracking

### 🔐 Authentification
- JWT token-based authentication
- Spotify OAuth integration
- Refresh token mechanism
- User whitelist management

### 📊 Cache & Performance
- Redis caching for randomized **Guess** game sessions

## 🛠️ Stack Technique
- .NET 9.0
- Entity Framework Core (ORM)
- PostgreSQL
- Redis
- MediatR (CQRS pattern)
- JWT Bearer Authentication
- Swagger/OpenAPI for API documentation

## 🏗️ Architecture

The project follows **Clean Architecture** principles with a clear separation of concerns:

- **Viberz.API** : HTTP entry points (Controllers)
- **Viberz.Application** : Business logic, services, DTOs, CQRS with MediatR
- **Viberz.Domain** : Domain entities, enums, and models
- **Viberz.Infrastructure** : Data access, external services, configurations

### Patterns Used
- **CQRS** with MediatR for command/query separation
- **Strategy Pattern** for different game modes
- **Factory Pattern** for strategy instantiation

### 📋 Prérequis
- .NET 9.0 SDK or higher
- PostgreSQL 14+
- Redis (local or cloud)
- Spotify Developer account

## 🔧 Installation

1. **Clone the repository**
```bash
git clone https://github.com/ThomasBfrd/Viberz_Backend.git
cd viberz-back
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Configure environment variables** (see next section)

4. **Run the application**
```bash
dotnet run --project Viberz.API
```

The API will be available at ```https://localhost:7053```.

## 🚀 Environment Configuration

Create a file ```appsettings.Development.json``` in the ```Viberz.API``` project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=viberz;Username=YOUR_USER;Password=YOUR_PASSWORD"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppSettings": {
    "AppName": "Viberz_Back",
    "Version": "1.0.0"
  },
  "AllowedOrigins": [
    "http://localhost:5173"
  ],
  "Spotify": {
    "ClientId": "YOUR_SPOTIFY_CLIENT_ID",
    "ClientSecret": "YOUR_SPOTIFY_CLIENT_SECRET"
  },
  "Jwt": {
    "Secret": "YOUR_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "ViberzAPI",
    "Audience": "ViberzClient"
  },
  "Redis": {
    "Host": "YOUR_REDIS_HOST",
    "Port": 17254,
    "User": "default",
    "Password": "YOUR_REDIS_PASSWORD",
    "Ssl": true
  }
}
```

## Spotify API Setup

1. Create an app on [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
2. Retrieve your **Client ID** and **Client Secret**
3. Configure the **Redirect URIs**

## Database Setup

**PostgreSQL local**
```bash
# Créer la base de données
createdb viberz
```
**Note** : Migrations are applied automatically at startup via `Program.cs`.  

**Whitelist utilisateurs** : Add allowed Spotify emails manually in the `whitelist` table.

## 📡 Main Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh token
- `POST /api/auth/spotify/exchange` - Exchange Spotify code

### User
- `GET /api/user/profile` - Get user profile
- `PUT /api/user/profile` - Update profile
- `GET /api/user/stats` - User stats

### Guess Game
- `GET /api/guess?gameType={type}&definedGenre={genres}` - Start a game

### Xp History
- `GET /api/xp-history/add-history-game` - Retrieve XP from games

### Artists & Genres
- `GET /api/artists/top` - Top artists
- `GET /api/genres` - Genres list

## 📄 License

This project is licensed under the MIT License – see [LICENSE](LICENSE) for details.

## ⚠️ Notes Importantes

Portfolio project with limitations:
- Spotify API: Maximum 25 users (manually added)
- Not production-ready: For demonstration only

To run this project, you need:
- Your own Spotify Developer credentials
- PostgreSQL and Redis (local or cloud)
- Manual user whitelist in the database

**Frontend repository** : [Viberz Frontend](https://github.com/ThomasBfrd/Viberz_Front)
