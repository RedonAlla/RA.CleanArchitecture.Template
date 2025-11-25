# TodoApp with Weather Integration

A clean architecture .NET Todo application with external weather API integration.

## Features

- ✅ Todo item management (CRUD operations)
- ✅ Weather forecast integration for due dates
- ✅ SQLite database
- ✅ RESTful API
- ✅ Swagger documentation

## Project Structure

```
TodoApp/
├── TodoApp.sln
├── TodoApp.Domain/
│   ├── Common/
│   │   └── BaseEntity.cs
│   ├── Entities/
│   │   ├── TodoItem.cs
│   │   └── WeatherInfo.cs
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── ITodoRepository.cs
│   │   ├── IWeatherRepository.cs
│   │   └── IWeatherService.cs
│   └── TodoApp.Domain.csproj
├── TodoApp.Application/
│   ├── DTOs/
│   │   ├── CreateTodoItemDto.cs
│   │   ├── TodoItemDto.cs
│   │   ├── TodoItemWithWeatherDto.cs
│   │   ├── UpdateTodoItemDto.cs
│   │   ├── WeatherApiResponse.cs
│   │   └── WeatherInfoDto.cs
│   ├── Interfaces/
│   │   ├── IExternalWeatherApiClient.cs
│   │   ├── ITodoService.cs
│   │   └── IWeatherService.cs
│   ├── Mappings/
│   │   └── MappingProfile.cs
│   ├── Services/
│   │   ├── TodoService.cs
│   │   └── WeatherService.cs
│   ├── DependencyInjection.cs
│   └── TodoApp.Application.csproj
├── TodoApp.Infrastructure/
│   ├── Data/
│   │   └── TodoDbContext.cs
│   ├── External/
│   │   └── OpenWeatherMapClient.cs
│   ├── Repositories/
│   │   ├── TodoRepository.cs
│   │   └── WeatherRepository.cs
│   ├── DependencyInjection.cs
│   └── TodoApp.Infrastructure.csproj
├── TodoApp.WebApi/
│   ├── Controllers/
│   │   ├── TodoController.cs
│   │   └── WeatherController.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── Program.cs
│   └── TodoApp.WebApi.csproj
└── README.md
```

## Setup

1. Restore packages: `dotnet restore`
2. Apply migrations: `dotnet ef migrations add InitialCreate --project TodoApp.Infrastructure --startup-project TodoApp.WebApi`
3. Update database: `dotnet ef database update --project TodoApp.Infrastructure --startup-project TodoApp.WebApi`
4. Run: `dotnet run --project TodoApp.WebApi`

## API Endpoints

- `GET /api/todo` - Get all todos
- `GET /api/todo/{id}/with-weather` - Get todo with weather info
- `GET /api/weather/todo/{id}` - Get weather for specific todo
- `GET /api/weather/forecast?location={}&date={}` - Get weather forecast

## Configuration

Add your weather API key in `appsettings.json`:
```json
"WeatherApi": {
  "ApiKey": "your_actual_api_key"
}