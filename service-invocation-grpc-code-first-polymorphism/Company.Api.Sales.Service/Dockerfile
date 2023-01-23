#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Company.Api.Sales.Service/Company.Api.Sales.Service.csproj", "Company.Api.Sales.Service/"]
COPY ["Company.Manager.Sales.Interface/Company.Manager.Sales.Interface.csproj", "Company.Manager.Sales.Interface/"]
COPY ["Company.Framework/Company.Framework.csproj", "Company.Framework/"]
RUN dotnet restore "Company.Api.Sales.Service/Company.Api.Sales.Service.csproj"
COPY . .
WORKDIR "/src/Company.Api.Sales.Service"
RUN dotnet build "Company.Api.Sales.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Company.Api.Sales.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Api.Sales.Service.dll"]