#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/vita.Web.Public/vita.Web.Public.csproj", "src/vita.Web.Public/"]
COPY ["src/vita.Web.Core/vita.Web.Core.csproj", "src/vita.Web.Core/"]
COPY ["src/vita.Application/vita.Application.csproj", "src/vita.Application/"]
COPY ["src/vita.Application.Shared/vita.Application.Shared.csproj", "src/vita.Application.Shared/"]
COPY ["src/vita.Core.Shared/vita.Core.Shared.csproj", "src/vita.Core.Shared/"]
COPY ["src/vita.Core/vita.Core.csproj", "src/vita.Core/"]
COPY ["src/vita.EntityFrameworkCore/vita.EntityFrameworkCore.csproj", "src/vita.EntityFrameworkCore/"]
COPY ["src/vita.GraphQL/vita.GraphQL.csproj", "src/vita.GraphQL/"]
RUN dotnet restore "src/vita.Web.Public/vita.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/vita.Web.Public"
RUN dotnet build "vita.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vita.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vita.Web.Public.dll"]
