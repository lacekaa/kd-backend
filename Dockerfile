# ---- Base runtime image ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# (1) Switch to non‑root user if you have APP_UID set up:
USER $APP_UID

WORKDIR /app

# (2) Expose the ports your app listens on (adjust if needed)
EXPOSE 8080
EXPOSE 8081

# (3) Create the DataFiles folder and declare it as a volume
RUN mkdir -p /app/DataFiles
VOLUME [ "/app/DataFiles" ]

# ---- Build image ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# copy only csproj and restore first (cache layer)
COPY ["kd-backend/kd-backend.csproj", "kd-backend/"]
RUN dotnet restore "kd-backend/kd-backend.csproj"

# copy everything else and build
COPY . .
WORKDIR "/src/kd-backend"
RUN dotnet build "kd-backend.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# ---- Publish ----
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "kd-backend.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ---- Final image ----
FROM base AS final
WORKDIR /app

# copy published output
COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "kd-backend.dll"]

