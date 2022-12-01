#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Wifi.PlayListEditor.Service/Wifi.PlayListEditor.Service.csproj", "Wifi.PlayListEditor.Service/"]
RUN dotnet restore "Wifi.PlayListEditor.Service/Wifi.PlayListEditor.Service.csproj"
COPY . .
WORKDIR "/src/Wifi.PlayListEditor.Service"
RUN dotnet build "Wifi.PlayListEditor.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wifi.PlayListEditor.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wifi.PlayListEditor.Service.dll"]