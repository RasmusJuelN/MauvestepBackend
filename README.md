# Mauve Step — Community Backend

This repository contains the backend API for **Mauve Step**, created as part of my final apprenticeship project in the **Data Technician education programme with a specialization in programming**.

The full project consists of three connected parts: a frontend application hosted in a separate repository, this backend API, and the **Mauve Step** game developed by my project partner. The backend serves as the data layer and business logic foundation for the community platform, handling everything from user authentication to forum interactions and leaderboard management.

The API is designed to support the player community platform by exposing endpoints for features such as forums, leaderboards, game mechanics documentation, support pages, user profiles, and news. Its goal is to provide a reliable and secure data backbone that powers the overall player experience outside of gameplay.

---

## Tech Stack

- **Runtime:** .NET 8 / ASP.NET Core Web API
- **Database:** PostgreSQL (via Entity Framework Core + Npgsql)
- **Authentication:** JWT Bearer tokens
- **Documentation:** Swagger / OpenAPI
- **Containerisation:** Docker
- **Hosting:** Render.com

---

## Core Features

| Area | Description |
|---|---|
| **Authentication** | User registration and login with JWT token issuance |
| **Users** | User profile management |
| **News** | Create, read, update and delete news articles |
| **Forum** | Categories, threads, posts, and comments with rating support |
| **Highscores** | Leaderboard entries for the game |
| **Characters & Abilities** | Game mechanics documentation exposed through the API |
| **Support Tickets** | Players can submit and track support requests |
| **Bug Reports** | In-platform bug reporting for the game |
| **FAQ** | Frequently asked questions managed through the API |
| **Feedback** | General player feedback collection |

---

## Project Structure

```
MauvestepBackend/
├── API/                    # ASP.NET Core Web API project
│   ├── Controllers/        # HTTP endpoint controllers
│   ├── DTOs/               # Data Transfer Objects
│   ├── Interfaces/         # Service interfaces
│   ├── Services/           # Business logic layer
│   └── Program.cs          # Application entry point and DI configuration
├── Database/               # Data access layer
│   ├── Context/            # EF Core DbContext
│   ├── Interfaces/         # Repository interfaces
│   ├── Migrations/         # EF Core database migrations
│   ├── Models/             # Entity models
│   └── Repositories/       # Repository implementations
└── Dockerfile              # Docker build configuration
```
