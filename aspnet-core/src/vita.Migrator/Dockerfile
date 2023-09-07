#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/vita.Migrator/vita.Migrator.csproj", "src/vita.Migrator/"]
COPY ["src/vita.EntityFrameworkCore/vita.EntityFrameworkCore.csproj", "src/vita.EntityFrameworkCore/"]
COPY ["src/vita.Core/vita.Core.csproj", "src/vita.Core/"]
COPY ["src/vita.Core.Shared/vita.Core.Shared.csproj", "src/vita.Core.Shared/"]
RUN dotnet restore "src/vita.Migrator/vita.Migrator.csproj"
COPY . .
WORKDIR "/src/src/vita.Migrator"
RUN dotnet build "vita.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vita.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vita.Migrator.dll", "-s"]
