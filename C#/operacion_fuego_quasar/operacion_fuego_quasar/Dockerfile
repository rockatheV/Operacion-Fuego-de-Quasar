#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["operacion_fuego_quasar/operacion_fuego_quasar.csproj", "operacion_fuego_quasar/"]
RUN dotnet restore "operacion_fuego_quasar/operacion_fuego_quasar.csproj"
COPY . .
WORKDIR "/src/operacion_fuego_quasar"
RUN dotnet build "operacion_fuego_quasar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "operacion_fuego_quasar.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "operacion_fuego_quasar.dll"]