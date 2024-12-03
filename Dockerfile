# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PolarisContacts.ConsumerService/PolarisContacts.ConsumerService.csproj", "PolarisContacts.ConsumerService/"]
COPY ["PolarisContacts.ConsumerService.Application/PolarisContacts.ConsumerService.Application.csproj", "PolarisContacts.ConsumerService.Application/"]
COPY ["PolarisContacts.ConsumerService.CrossCutting.Helpers/PolarisContacts.ConsumerService.CrossCutting.Helpers.csproj", "PolarisContacts.ConsumerService.CrossCutting.Helpers/"]
COPY ["PolarisContacts.ConsumerService.Domain/PolarisContacts.ConsumerService.Domain.csproj", "PolarisContacts.ConsumerService.Domain/"]
COPY ["PolarisContacts.ConsumerService.CrossCutting.DependencyInjection/PolarisContacts.ConsumerService.CrossCutting.DependencyInjection.csproj", "PolarisContacts.ConsumerService.CrossCutting.DependencyInjection/"]
COPY ["PolarisContacts.ConsumerService.Infrastructure/PolarisContacts.ConsumerService.Infrastructure.csproj", "PolarisContacts.ConsumerService.Infrastructure/"]
RUN dotnet restore "./PolarisContacts.ConsumerService/PolarisContacts.ConsumerService.csproj"
COPY . .
WORKDIR "/src/PolarisContacts.ConsumerService"
RUN dotnet build "./PolarisContacts.ConsumerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PolarisContacts.ConsumerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolarisContacts.ConsumerService.dll"]