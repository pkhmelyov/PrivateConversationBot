FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["PrivateConversationBot.Web/PrivateConversationBot.Web.csproj", "PrivateConversationBot.Web/"]
RUN dotnet restore "PrivateConversationBot.Web/PrivateConversationBot.Web.csproj"
COPY . .
WORKDIR "/src/PrivateConversationBot.Web"
RUN dotnet build "PrivateConversationBot.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrivateConversationBot.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrivateConversationBot.Web.dll"]
