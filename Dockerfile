# ─── Stage 1: Build & Publish ─────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# 1. Set workdir
WORKDIR /src

# 2. Copy everything and restore+publish
COPY . .

# 3. Restore and publish in Release mode to /app/publish
#    dotnet publish will compile, package, and include all dependencies.
RUN dotnet publish "kd-backend.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ─── Stage 2: Runtime ───────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# 4. Run as non-root if you have APP_UID (optional)
USER $APP_UID

WORKDIR /app

# 5. Ensure the DataFiles folder exists and declare it a volume
RUN mkdir -p /app/DataFiles
VOLUME [ "/app/DataFiles" ]

# 6. Copy the published app from the build image
COPY --from=build /app/publish ./

# 7. Expose your HTTP/HTTPS ports
EXPOSE 8080
EXPOSE 8081

# 8. Launch the app
ENTRYPOINT ["dotnet", "kd-backend.dll"]
