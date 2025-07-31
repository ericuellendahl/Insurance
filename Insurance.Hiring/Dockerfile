# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Insurance.Hiring/Insurance.Hiring.csproj", "Insurance.Hiring/"]
COPY ["Insurance.Hiring.Adapter/Insurance.Hiring.Adapter.csproj", "Insurance.Hiring.Adapter/"]
COPY ["Insurance.Hiring.Domain/Insurance.Hiring.Domain.csproj", "Insurance.Hiring.Domain/"]
COPY ["Insurance.Shared/Insurance.Shared.csproj", "Insurance.Shared/"]
COPY ["Insurance.Hiring.Infra/Insurance.Hiring.Infra.csproj", "Insurance.Hiring.Infra/"]
COPY ["Insurance.Hiring.Application/Insurance.Hiring.Application.csproj", "Insurance.Hiring.Application/"]
COPY ["Insurance.ServiceDefaults/Insurance.ServiceDefaults.csproj", "Insurance.ServiceDefaults/"]
RUN dotnet restore "./Insurance.Hiring/Insurance.Hiring.csproj"
COPY . .
WORKDIR "/src/Insurance.Hiring"
RUN dotnet build "./Insurance.Hiring.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Insurance.Hiring.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Insurance.Hiring.dll"]