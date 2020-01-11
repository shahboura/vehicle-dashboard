FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY vehicles-dashboard.sln .
COPY ./src/Dashboard.CloudStorage/Dashboard.CloudStorage.csproj /src/Dashboard.CloudStorage/
COPY ./src/Dashboard.Api/Dashboard.Api.csproj /src/Dashboard.Api/
COPY ./src/Dashboard.Admin.Api/Dashboard.Admin.Api.csproj /src/Dashboard.Admin.Api/ 
COPY ./tests/UnitTests/Dashboard.Api.Tests/Dashboard.Api.Tests.csproj /tests/UnitTests/Dashboard.Api.Tests/

RUN dotnet restore

COPY . .
WORKDIR /src/Dashboard.Api/
RUN dotnet publish --runtime linux-musl-x64 -c Release -o /app /p:PublishTrimmed=true

# Runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "/app/Dashboard.Api.dll" ]