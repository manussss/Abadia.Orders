FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Abadia.Orders.API/Abadia.Orders.API.csproj", "src/Abadia.Orders.API/"]
COPY ["src/Abadia.Orders.Application/Abadia.Orders.Application.csproj", "src/Abadia.Orders.Application/"]
COPY ["src/Abadia.Orders.Domain/Abadia.Orders.Domain.csproj", "src/Abadia.Orders.Domain/"]
COPY ["src/Abadia.Orders.Shared/Abadia.Orders.Shared.csproj", "src/Abadia.Orders.Shared/"]
COPY ["src/Abadia.Orders.Infra.CrossCutting/Abadia.Orders.Infra.CrossCutting.csproj", "src/Abadia.Orders.Infra.CrossCutting/"]
COPY ["src/Abadia.Orders.Infra/Abadia.Orders.Infra.csproj", "src/Abadia.Orders.Infra/"]
RUN dotnet restore "src/Abadia.Orders.API/Abadia.Orders.API.csproj"
COPY . .
WORKDIR "/src/src/Abadia.Orders.API"
RUN dotnet build "Abadia.Orders.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abadia.Orders.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Abadia.Orders.API.dll"]