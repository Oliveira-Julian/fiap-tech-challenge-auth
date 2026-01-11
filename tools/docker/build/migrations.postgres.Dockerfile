ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:9.0-alpine

FROM ${DOTNET_SDK} AS build
WORKDIR /src
COPY ["FoodChallenge.Auth.Api/FoodChallenge.Auth.Api.csproj", "FoodChallenge.Auth.Api/"]
RUN dotnet restore "FoodChallenge.Auth.Api/FoodChallenge.Auth.Api.csproj"
COPY . .

FROM build as migrations
WORKDIR /src
RUN dotnet tool install --version 9.0.5 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --project FoodChallenge.Auth.Api --startup-project
