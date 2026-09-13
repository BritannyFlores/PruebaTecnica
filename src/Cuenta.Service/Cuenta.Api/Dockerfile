FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Cuenta.Service/Cuenta.Api/Cuenta.Api.csproj", "src/Cuenta.Service/Cuenta.Api/"]
COPY ["src/Cuenta.Service/Cuenta.Application/Cuenta.Application.csproj", "src/Cuenta.Service/Cuenta.Application/"]
COPY ["src/Cuenta.Service/Cuenta.Domain/Cuenta.Domain.csproj", "src/Cuenta.Service/Cuenta.Domain/"]
COPY ["src/Cuenta.Service/Cuenta.Infrastructure/Cuenta.Infrastructure.csproj", "src/Cuenta.Service/Cuenta.Infrastructure/"]
COPY ["src/Shared.Contracts/Shared.Contracts.csproj", "src/Shared.Contracts/"]

RUN dotnet restore "src/Cuenta.Service/Cuenta.Api/Cuenta.Api.csproj"

COPY . .
WORKDIR "/src/src/Cuenta.Service/Cuenta.Api"
RUN dotnet build "Cuenta.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cuenta.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cuenta.Api.dll"]