FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5214

ENV ASPNETCORE_URLS=http://+:5214

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Api/RealTimeProductCatalog.Api/RealTimeProductCatalog.Api.csproj", "src/Api/RealTimeProductCatalog.Api/"]
RUN dotnet restore "src/Api/RealTimeProductCatalog.Api/RealTimeProductCatalog.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/RealTimeProductCatalog.Api"
RUN dotnet build "RealTimeProductCatalog.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "RealTimeProductCatalog.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeProductCatalog.Api.dll"]
