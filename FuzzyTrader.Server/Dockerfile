FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FuzzyTrader.Server/FuzzyTrader.Server.csproj", "FuzzyTrader.Server/"]
RUN dotnet restore "FuzzyTrader.Server/FuzzyTrader.Server.csproj"
COPY . .
WORKDIR "/src/FuzzyTrader.Server"
RUN dotnet build "FuzzyTrader.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FuzzyTrader.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FuzzyTrader.Server.dll"]
