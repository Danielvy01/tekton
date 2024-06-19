#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Microservicio Configuracion/Tekton.Configuration.WebApi/Tekton.Configuration.WebApi.csproj", "Microservicio Configuracion/Tekton.Configuration.WebApi/"]
COPY ["CrossProject/Tekton.Service.Common/Tekton.Service.Common.csproj", "CrossProject/Tekton.Service.Common/"]
COPY ["CrossProject/Tekton.Application.Common/Tekton.Application.Common.csproj", "CrossProject/Tekton.Application.Common/"]
COPY ["CrossProject/Tekton.Caching.Common/Tekton.Caching.Common.csproj", "CrossProject/Tekton.Caching.Common/"]
COPY ["CrossProject/Tekton.Elastic.LogCommon/Tekton.Elastic.LogCommon.csproj", "CrossProject/Tekton.Elastic.LogCommon/"]
COPY ["CrossProject/Tekton.Seguridad.Common/Tekton.Seguridad.Common.csproj", "CrossProject/Tekton.Seguridad.Common/"]
COPY ["Microservicio Configuracion/Tekton.Configuration.Application/Tekton.Configuration.Application.csproj", "Microservicio Configuracion/Tekton.Configuration.Application/"]
COPY ["Microservicio Configuracion/Tekton.Configuration.Infraestructure/Tekton.Configuration.Infraestructure.csproj", "Microservicio Configuracion/Tekton.Configuration.Infraestructure/"]
COPY ["Microservicio Configuracion/Tekton.Configuration.Damain/Tekton.Configuration.Damain.csproj", "Microservicio Configuracion/Tekton.Configuration.Damain/"]
RUN dotnet restore "Microservicio Configuracion/Tekton.Configuration.WebApi/Tekton.Configuration.WebApi.csproj"
COPY . .
WORKDIR "/src/Microservicio Configuracion/Tekton.Configuration.WebApi"
RUN dotnet build "Tekton.Configuration.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tekton.Configuration.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tekton.Configuration.WebApi.dll"]