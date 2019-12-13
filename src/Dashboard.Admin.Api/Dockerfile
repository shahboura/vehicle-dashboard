FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src/
COPY vehicles-dashboard.sln .
COPY ./src/Dashboard.CloudStorage/Dashboard.CloudStorage.csproj /src/Dashboard.CloudStorage/
COPY ./src/Dashboard.Api/Dashboard.Api.csproj /src/Dashboard.Api/
COPY ./src/Dashboard.Admin.Api/Dashboard.Admin.Api.csproj /src/Dashboard.Admin.Api/ 
COPY ./tests/UnitTests/Dashboard.Api.Tests/Dashboard.Api.Tests.csproj /src/Dashboard.Api.Tests/

RUN dotnet restore

COPY . .
WORKDIR /src/Dashboard.Admin.Api/
RUN dotnet publish -c Release -o /app

# Runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "/app/Dashboard.Admin.Api.dll" ]