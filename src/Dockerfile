FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Catalog.API/Catalog.API.csproj", "Catalog.API/"]
COPY ["Catalog.Core/Catalog.Core.csproj", "Catalog.Core/"]
COPY ["Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalog.Infrastructure/"]

RUN dotnet restore "Catalog.API/Catalog.API.csproj"

COPY . .
WORKDIR "/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

USER app 

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]