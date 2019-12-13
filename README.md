[![pipeline status](https://gitlab.com/shahboura/github-vehicle-dashboard/badges/master/pipeline.svg)](https://gitlab.com/shahboura/github-vehicle-dashboard/commits/master) [![coverage report](https://gitlab.com/shahboura/github-vehicle-dashboard/badges/master/coverage.svg)](https://gitlab.com/shahboura/github-vehicle-dashboard/commits/master)

# Introduction

This is a sample project combining .net, docker & gitlab. Idea is to provide API and admin service viewing vehicle list and their status. Status of the vehicles are changing depending on ping request.

## Requirements

If using [docker](#docker) or azure storage emulator not installed, storage connection string is needed.
Follow this [link](https://docs.microsoft.com/en-us/azure/storage/common/storage-quickstart-create-account?tabs=azure-portal) to create one.

## Installation

We can run the services using either [CLI](#CommandLine) or [docker](#docker).

### CommandLine

* Powershell

```powershell
# Running dashboard.admin
dotnet run --project .\Dashboard.Admin.Api --AzureCloudStorage "UseDevelopmentStorage=true"

# Running dashboard.api
dotnet run --project .\Dashboard.Api --AzureCloudStorage "UseDevelopmentStorage=true"
```

*Note: If azure storage emulator installed, we can use `"UseDevelopmentStorage=true"`*

* Bash
CommandLine Linus / MacOSX

```bash
# Running dashboard.admin
dotnet run --project Dashboard.Admin.Api --AzureCloudStorage "<CONNECTION-STRING>"

# Running dashboard.api
dotnet run --project Dashboard.Api --AzureCloudStorage "CONNECTION-STRING"
```

### Docker

* Set azure storage connection string in development.env `AzureCloudStorage=VALUE`
* run `docker-compose up`

## Endpoints

* Swagger: <http://localhost:5001/swagger/>
* Vehicles: GET <http://localhost:5001/api/vehicles>
* Vehicles / owner: GET <http://localhost:5001/api/owners/{ownerId}/vehicles>
* Swagger admin: <http://localhost:5003/swagger/>
* Seed data: POST <http://localhost:5003/api/seed>
* Ping vehicles: POST <http://localhost:5003/api/vehicles/ping>

## Reference links

* [GitLab CI Documentation](https://docs.gitlab.com/ee/ci/)
