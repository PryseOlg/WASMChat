FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/WASMChat.Server.csproj", "Server/"]
COPY ["Client/WASMChat.Client.csproj", "Client/"]
COPY ["Shared/WASMChat.Shared.csproj", "Shared/"]
COPY ["Data/WASMChat.Data.csproj", "Data/"]
RUN dotnet restore "Server/WASMChat.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "WASMChat.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WASMChat.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WASMChat.Server.dll"]
