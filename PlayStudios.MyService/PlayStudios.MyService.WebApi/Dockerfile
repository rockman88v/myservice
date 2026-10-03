# syntax=docker/dockerfile:1
#
# NOTE: TargetFramework is netcoreapp3.1 (End-Of-Life, no more security patches from Microsoft).
# This Dockerfile builds it as-is per request (no source/csproj changes). Upgrading the
# TargetFramework to a supported LTS (e.g. net8.0) is strongly recommended as a follow-up.
#
# Pin tags to a specific digest in CI (docker inspect --format='{{index .RepoDigests 0}}')
# once this image is built once, for fully reproducible/immutable builds.

# --- Build stage: SDK image, only used to compile/publish, discarded after ---
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copy only the csproj first so `dotnet restore` is cached in its own layer and is only
# re-run when dependencies actually change, not on every source edit.
COPY PlayStudios.MyService.WebApi.csproj .
RUN dotnet restore "PlayStudios.MyService.WebApi.csproj"

# Now copy the rest of the source and publish (Release, framework-dependent, no re-restore).
COPY . .
RUN dotnet publish "PlayStudios.MyService.WebApi.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    --no-self-contained \
    /p:UseAppHost=false

# --- Runtime stage: ASP.NET runtime only (no SDK/build tools) — smaller, smaller attack surface ---
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app

# aspnet:3.1 base image runs as root by default — create an unprivileged user to run as.
RUN groupadd --gid 1000 app \
    && useradd --uid 1000 --gid app --no-create-home --shell /usr/sbin/nologin app

COPY --from=build --chown=app:app /app/publish .

USER app
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_EnableDiagnostics=0 \
    DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 8080

ENTRYPOINT ["dotnet", "PlayStudios.MyService.WebApi.dll"]
