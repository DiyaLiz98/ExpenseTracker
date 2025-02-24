# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy project files and restore dependencies
COPY . .
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Use the .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy built files from the previous step
COPY --from=build /app/out .

# Expose the port your API runs on
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "ExpenseTracker.dll"]
