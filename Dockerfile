#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SWP391.APIs/SWP391.APIs.csproj", "SWP391.APIs/"]
COPY ["SWP391.BLL/SWP391.BLL.csproj", "SWP391.BLL/"]
COPY ["SWP391.DAL/SWP391.DAL.csproj", "SWP391.DAL/"]
RUN dotnet restore "./SWP391.APIs/SWP391.APIs.csproj"
COPY . .
WORKDIR "/src/SWP391.APIs"
RUN dotnet build "./SWP391.APIs.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SWP391.APIs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SWP391.APIs.dll"]