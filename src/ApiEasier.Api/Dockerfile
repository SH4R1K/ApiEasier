# Базовый образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Копируем все .csproj файлы сразу, чтобы ускорить кеширование
COPY ApiEasier.Api/ApiEasier.Api.csproj ApiEasier.Api/
COPY ApiEasier.Bll/ApiEasier.Bll.csproj ApiEasier.Bll/
COPY ApiEasier.Dal/ApiEasier.Dal.csproj ApiEasier.Dal/
COPY ApiEasier.Dm/ApiEasier.Dm.csproj ApiEasier.Dm/
COPY ApiEasier.Logger/ApiEasier.Logger.csproj ApiEasier.Logger/

# Восстанавливаем зависимости
WORKDIR /source/ApiEasier.Api
RUN dotnet restore

# Копируем весь код проекта
WORKDIR /source
COPY . .

# Сборка и публикация
RUN dotnet publish -c Release -o /app --no-restore

# Базовый образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Копируем собранный код
COPY --from=build /app ./

# Запуск приложения
ENTRYPOINT ["dotnet", "ApiEasier.Api.dll"]
