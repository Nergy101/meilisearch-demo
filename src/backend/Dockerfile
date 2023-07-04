# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory.
WORKDIR /app

# Copy everything else and build app
COPY MeiliSearchDemo/. ./MeiliSearchDemo/
WORKDIR /app/MeiliSearchDemo

RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/MeiliSearchDemo/out .
ENTRYPOINT ["dotnet", "MeiliSearchDemo.dll"]
