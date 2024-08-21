#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MessageBroker.Kafka.Producer.Simulator/MessageBroker.Kafka.Producer.Simulator.csproj", "MessageBroker.Kafka.Producer.Simulator/"]
RUN dotnet restore "./MessageBroker.Kafka.Producer.Simulator/./MessageBroker.Kafka.Producer.Simulator.csproj"
COPY . .
WORKDIR "/src/MessageBroker.Kafka.Producer.Simulator"
RUN dotnet build "./MessageBroker.Kafka.Producer.Simulator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MessageBroker.Kafka.Producer.Simulator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageBroker.Kafka.Producer.Simulator.dll"]