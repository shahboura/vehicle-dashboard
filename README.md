[![pipeline status](https://gitlab.com/shahboura/github-vehicle-dashboard/badges/master/pipeline.svg)](https://gitlab.com/shahboura/github-vehicle-dashboard/commits/master)

# Introduction

This is a simple pipeline example for a .NET Core application, showing just
how easy it is to get up and running with .NET development using GitLab.

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
* [.NET Hello World tutorial](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/)

If you're new to .NET you'll want to check out the tutorial, but if you're
already a seasoned developer considering building your own .NET app with GitLab,
this should all look very familiar.

## What's contained in this project

The root of the repository contains the out of the `dotnet new console` command,
which generates a new console application that just prints out "Hello, World."
It's a simple example, but great for demonstrating how easy GitLab CI is to
use with .NET. Check out the `Program.cs` and `dotnetcore.csproj` files to
see how these work.

In addition to the .NET Core content, there is a ready-to-go `.gitignore` file
sourced from the the .NET Core [.gitignore](https://github.com/dotnet/core/blob/master/.gitignore). This
will help keep your repository clean of build files and other configuration.

Finally, the `.gitlab-ci.yml` contains the configuration needed for GitLab
to build your code. Let's take a look, section by section.

First, we note that we want to use the official Microsoft .NET SDK image
to build our project.

```YAML
image: mcr.microsoft.com/dotnet/core/sdk:3.0
```

We're defining two stages here: `build`, and `test`. As your project grows
in complexity you can add more of these.

```YAML
stages:
    - build
    - test
```

Next, we define our build job which simply runs the `dotnet build` command and
identifies the `bin` folder as the output directory. Anything in the `bin` folder
will be automatically handed off to future stages, and is also downloadable through
the web UI.

```YAML
build:
    stage: build
    script:
        - "dotnet build"
    artifacts:
      paths:
        - bin/
```

Similar to the build step, we get our test output simply by running `dotnet test`.

```YAML
test:
    stage: test
    script:
        - "dotnet test"
```

This should be enough to get you started. There are many, many powerful options 
for your `.gitlab-ci.yml`. You can read about them in our documentation 
[here](https://docs.gitlab.com/ee/ci/yaml/).