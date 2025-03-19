# Sử dụng runtime .NET 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Sử dụng SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InnovexGlobal.csproj", "./"]
RUN dotnet restore "./InnovexGlobal.csproj"

COPY . .
RUN dotnet publish "InnovexGlobal.csproj" -c Release -o /app/publish

# Chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
CMD ["dotnet", "InnovexGlobal.dll"]
