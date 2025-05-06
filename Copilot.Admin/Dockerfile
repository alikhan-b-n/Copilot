FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Copilot.Admin/Copilot.Admin.csproj", "Copilot.Admin/"]
RUN dotnet restore "Copilot.Admin/Copilot.Admin.csproj"
COPY . .
WORKDIR "/src/Copilot.Admin"
RUN dotnet build "Copilot.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Copilot.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["Copilot.Admin/nginx.conf", "/etc/nginx/nginx.conf"]
