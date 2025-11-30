# Multi-stage Dockerfile for .NET 8 Web API (works with typical repo layouts)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire repository (adjust for large repos if needed)
COPY . .

# Find the first .csproj, restore and publish it to /app/publish
RUN DOTNET_PROJECT="$(find . -maxdepth 4 -name '*.csproj' | head -n 1)" \
 && if [ -z "$DOTNET_PROJECT" ]; then echo "No .csproj found"; exit 1; fi \
 && dotnet restore "$DOTNET_PROJECT" \
 && dotnet publish "$DOTNET_PROJECT" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Run the first DLL found in the publish folder (avoids hardcoding assembly name)
ENTRYPOINT ["sh", "-c", "dotnet $(ls -1 *.dll | head -n 1)"]