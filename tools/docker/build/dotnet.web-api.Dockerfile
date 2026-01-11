ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:9.0-alpine
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:9.0-alpine

FROM ${DOTNET_RUNTIME} AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5011

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "FoodChallenge.Auth.Api/FoodChallenge.Auth.Api.csproj"
WORKDIR "/src/FoodChallenge.Auth.Api"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FoodChallenge.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodChallenge.Auth.Api.dll"]
