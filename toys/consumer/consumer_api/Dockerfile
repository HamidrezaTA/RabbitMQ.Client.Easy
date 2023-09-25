# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj and restore any dependencies (if applicable)
COPY ./toys/consumer/consumer_api/*.csproj ./toys/consumer/consumer_api/
COPY ./toys/common/*.csproj ./toys/common/
COPY ./src/*.csproj ./src/

RUN dotnet restore ./toys/consumer/consumer_api/consumer_api.csproj

# Copy the remaining source code
COPY ./toys/consumer/consumer_api/ ./toys/consumer/consumer_api/
COPY ./toys/common/ ./toys/common/
COPY ./src/ ./src/

# Build the application
RUN dotnet publish ./toys/consumer/consumer_api/consumer_api.csproj -c Release -o out

# Use the official .NET Runtime image for the final image
FROM mcr.microsoft.com/dotnet/runtime:7.0

# Set the working directory in the container
WORKDIR /app

# Copy the built application from the build environment
COPY --from=build-env /app/out .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "consumer_api.dll"]