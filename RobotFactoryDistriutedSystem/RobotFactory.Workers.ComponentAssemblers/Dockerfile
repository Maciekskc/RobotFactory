#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RobotFactory.Workers.ComponentAssemblers/RobotFactory.Workers.ComponentAssemblers.csproj", "RobotFactory.Workers.ComponentAssemblers/"]
COPY ["RobotFactory.Workers.SharedComponents/RobotFactory.Workers.SharedComponents.csproj", "RobotFactory.Workers.SharedComponents/"]
COPY ["RobotFactory.DataAccessLayer/RobotFactory.DataAccessLayer.csproj", "RobotFactory.DataAccessLayer/"]
COPY ["RobotFactory.DataLayer/RobotFactory.DataLayer.csproj", "RobotFactory.DataLayer/"]
COPY ["RobotFactorySharedComponents/RobotFactory.SharedComponents.csproj", "RobotFactorySharedComponents/"]
RUN dotnet restore "./RobotFactory.Workers.ComponentAssemblers/./RobotFactory.Workers.ComponentAssemblers.csproj"
COPY . .
WORKDIR "/src/RobotFactory.Workers.ComponentAssemblers"
RUN dotnet build "./RobotFactory.Workers.ComponentAssemblers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RobotFactory.Workers.ComponentAssemblers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RobotFactory.Workers.ComponentAssemblers.dll"]