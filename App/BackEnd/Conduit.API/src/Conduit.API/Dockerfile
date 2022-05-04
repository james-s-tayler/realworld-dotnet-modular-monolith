#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build container
FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster AS build

# Copy the code into the container
WORKDIR /src
COPY ["src/Conduit.API/Conduit.API.csproj", "Conduit.API/"]

# NuGet restore
RUN dotnet restore "Conduit.API/Conduit.API.csproj"
COPY ["src/Conduit.API/", "Conduit.API/"]

# Build the API
WORKDIR "Conduit.API"
RUN dotnet build "Conduit.API.csproj" -c Release -o /app/build

# Publish it
FROM build AS publish
RUN dotnet publish "Conduit.API.csproj" -c Release -o /app/publish

# Make the final image for publishing
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conduit.API.dll"]
