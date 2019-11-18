FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src/
COPY alten-awesome-dashboard.sln .
COPY Dashboard.CloudStorage/Dashboard.CloudStorage.csproj /src/Dashboard.CloudStorage/
COPY Dashboard.Api/Dashboard.Api.csproj /src/Dashboard.Api/
COPY Dashboard.Api.Tests/Dashboard.Api.Tests.csproj /src/Dashboard.Api.Tests/
COPY Dashboard.Admin.Api/Dashboard.Admin.Api.csproj /src/Dashboard.Admin.Api/ 

RUN dotnet restore

COPY . .
WORKDIR /src/Dashboard.Api/
RUN dotnet publish -c Release -o /app

# Runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "/app/Dashboard.Api.dll" ]