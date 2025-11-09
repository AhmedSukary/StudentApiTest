# Use the official .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./

# Build the project in Release mode
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app/out ./

# Expose port 80
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "StudentApi.dll"]