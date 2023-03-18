#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 5000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ["A1.SAS.Api/A1.SAS.Api.csproj", "A1.SAS.Api/"]
COPY ["A1.SAS.Domain/A1.SAS.Domain.csproj", "A1.SAS.Domain/"]
COPY ["A1.SAS.Infrastructure/A1.SAS.Infrastructure.csproj", "A1.SAS.Infrastructure/"]

RUN dotnet restore "A1.SAS.Api/A1.SAS.Api.csproj"
COPY . .
WORKDIR "/app/A1.SAS.Api"
RUN dotnet build "A1.SAS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "A1.SAS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "A1.SAS.Api.dll", "--server.urls", "https://+:5000"]