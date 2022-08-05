#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM  mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["api/RewardsSystem.API/RewardsSystem.API.csproj", "RewardsSystem.API/"]
COPY ["core/RewardsSystem.Service/RewardsSystem.Service.csproj", "RewardsSystem.Service/"]
COPY ["core/RewardsSystem.Common/RewardsSystem.Common.csproj", "RewardsSystem.Common/"]
COPY ["core/RewardsSystem.Domain/RewardsSystem.Domain.csproj", "RewardsSystem.Domain/"]
COPY ["core/RewardsSystem.Shared/RewardsSystem.Shared.csproj", "RewardsSystem.Shared/"]
COPY ["infrastructure/RewardsSystem.Persistence/RewardsSystem.Persistence.csproj", "RewardsSystem.Persistence/"]
RUN dotnet restore "RewardsSystem.API/RewardsSystem.API.csproj"
COPY . .
WORKDIR "/src/RewardsSystem.API"
RUN dotnet build "RewardsSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RewardsSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RewardsSystem.API.dll"]