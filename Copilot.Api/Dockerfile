FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Copilot.Api/Copilot.Api.csproj", "Copilot.Api/"]
COPY ["Copilot.Utils/Copilot.Utils.csproj", "Copilot.Utils/"]
COPY ["Copilot.Ai/Copilot.Ai.csproj", "Copilot.Ai/"]
COPY ["Copilot.AI.Plugins/Copilot.AI.Plugins.csproj", "Copilot.AI.Plugins/"]
COPY ["Copilot.Ai.Utils/Copilot.Ai.Utils.csproj", "Copilot.Ai.Utils/"]
COPY ["Copilot.Experimental.Agents.Extensions/Copilot.Experimental.Agents.Extensions.csproj", "Copilot.Experimental.Agents.Extensions/"]
COPY ["Agents/Experimental.Agents.csproj", "Agents/"]
COPY ["Experimental.Agents.InternalUtilities/Experimental.Agents.InternalUtilities.csproj", "Experimental.Agents.InternalUtilities/"]
COPY ["Copilot.Altegio.Api/Copilot.Altegio.Api.csproj", "Copilot.Altegio.Api/"]
COPY ["Copilot.Infrastructure/Copilot.Infrastructure.csproj", "Copilot.Infrastructure/"]
COPY ["Copilot.ChatBotManagement/Copilot.ChatBotManagement.csproj", "Copilot.ChatBotManagement/"]
COPY ["Copilot.Contracts/Copilot.Contracts.csproj", "Copilot.Contracts/"]
COPY ["Copilot.WhatsApp.Api/Copilot.WhatsApp.Api.csproj", "Copilot.WhatsApp.Api/"]
COPY ["Copilot.MessageHandling/Copilot.MessageHandling.csproj", "Copilot.MessageHandling/"]
RUN dotnet restore "Copilot.Api/Copilot.Api.csproj"
COPY . .
WORKDIR "/src/Copilot.Api"
RUN dotnet build "Copilot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Copilot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Copilot.Api.dll"]
