#!/bin/sh
echo "Applying database migrations..."
dotnet ef database update --project ./ConwaysGameOfLife.Infrastructure --startup-project ./ConwaysGameOfLife.Api --configuration Release
echo "Starting the application..."
dotnet ConwaysGameOfLife.Api.dll