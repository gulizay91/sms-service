FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SmsService.API/SmsService.API.csproj", "src/SmsService.API/"]
COPY ["src/SmsService.Application/SmsService.Application.csproj", "src/SmsService.Application/"]
COPY ["src/SmsService.Infrastructure/SmsService.Infrastructure.csproj", "src/SmsService.Infrastructure/"]
RUN dotnet restore "src/SmsService.API/SmsService.API.csproj"
COPY . .
WORKDIR "/src/src/SmsService.API"
RUN dotnet build "./SmsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmsService.API.dll"]
