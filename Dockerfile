# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the solution file and restore dependencies
COPY backend.sln .
COPY ConwaysGameOfLife.Api/ConwaysGameOfLife.Api.csproj ./ConwaysGameOfLife.Api/
COPY ConwaysGameOfLife.Contracts/ConwaysGameOfLife.Contracts.csproj ./ConwaysGameOfLife.Contracts/
COPY ConwaysGameOfLife.Core/ConwaysGameOfLife.Core.csproj ./ConwaysGameOfLife.Core/
COPY ConwaysGameOfLife.Infrastructure/ConwaysGameOfLife.Infrastructure.csproj ./ConwaysGameOfLife.Infrastructure/
COPY ConwaysGameOfLife.Tests/ConwaysGameOfLife.Tests.csproj ./ConwaysGameOfLife.Tests/

# Restore the dependencies for all projects
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
WORKDIR /src/ConwaysGameOfLife.Api
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the build artifacts from the build stage
COPY --from=build /app/publish .

# Copy the SQLite database file (if it exists)
COPY ConwaysGameOfLife.Infrastructure/database.db ./database.db

# Copy the entrypoint script from the root directory
COPY entrypoint.sh ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

# Expose the port your API will run on
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["./entrypoint.sh"]