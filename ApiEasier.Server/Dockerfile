FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./*.csproj ./
RUN dotnet restore

COPY ./ ./
WORKDIR /source
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ApiEasier.Server.dll"]
