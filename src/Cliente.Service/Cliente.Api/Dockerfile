FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Cliente.Service/Cliente.Api/Cliente.Api.csproj", "src/Cliente.Service/Cliente.Api/"]
COPY ["src/Cliente.Service/Cliente.Application/Cliente.Application.csproj", "src/Cliente.Service/Cliente.Application/"]
COPY ["src/Cliente.Service/Cliente.Domain/Cliente.Domain.csproj", "src/Cliente.Service/Cliente.Domain/"]
COPY ["src/Cliente.Service/Cliente.Infrastructure/Cliente.Infrastructure.csproj", "src/Cliente.Service/Cliente.Infrastructure/"]
COPY ["src/Shared.Contracts/Shared.Contracts.csproj", "src/Shared.Contracts/"]

RUN dotnet restore "src/Cliente.Service/Cliente.Api/Cliente.Api.csproj"

COPY . .
WORKDIR "/src/src/Cliente.Service/Cliente.Api"
RUN dotnet build "Cliente.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cliente.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cliente.Api.dll"]