FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Insurance.Propost/Insurance.Propost.csproj", "Insurance.Propost/"]
COPY ["Insurance.Propost.Adapter/Insurance.Propost.Adapter.csproj", "Insurance.Propost.Adapter/"]
COPY ["Insurance.Propost.Domain/Insurance.Propost.Domain.csproj", "Insurance.Propost.Domain/"]
COPY ["Insurance.Propost.Infra/Insurance.Propost.Infra.csproj", "Insurance.Propost.Infra/"]
COPY ["Insurance.Propost.Application/Insurance.Propost.Application.csproj", "Insurance.Propost.Application/"]
COPY ["Insurance.ServiceDefaults/Insurance.ServiceDefaults.csproj", "Insurance.ServiceDefaults/"]
COPY ["Insurance.Shared/Insurance.Shared.csproj", "Insurance.Shared/"]
RUN dotnet restore "./Insurance.Propost/Insurance.Propost.csproj"
COPY . .
WORKDIR "/src/Insurance.Propost"
RUN dotnet build "./Insurance.Propost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Insurance.Propost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Insurance.Propost.dll"]