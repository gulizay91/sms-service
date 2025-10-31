# tests/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

# Tools
RUN dotnet tool install --global JetBrains.dotCover.CommandLineTools --version 2025.1.7 \
    && dotnet tool install --global dotnet-coverage \
    && dotnet tool install --global dotnet-reportgenerator-globaltool
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy sources
COPY . ./

# Restore & static analysis build ONLY for the unit test project
RUN dotnet restore -p:NuGetAudit=true -p:NuGetVulnerabilitySeverity=Moderate
RUN dotnet build tests/SmsService.UnitTests/SmsService.UnitTests.csproj -c $BUILD_CONFIGURATION /p:RunAnalyzersDuringBuild=true

# -------------------- TEST & COVERAGE --------------------
FROM build AS testrunner
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

# Run tests with built-in coverage (Cobertura)
RUN dotnet test tests/SmsService.UnitTests/SmsService.UnitTests.csproj \
    --no-build -c $BUILD_CONFIGURATION \
    --logger "trx;LogFileName=test_results.trx" \
    --results-directory /app/testresults \
    --collect:"XPlat Code Coverage"

# Generate reports (HTML, Cobertura, lcov, TextSummary)
RUN reportgenerator -reports:/app/testresults/**/coverage.cobertura.xml \
    -targetdir:/app/testresults/coverage-report \
    -reporttypes:"Html;Cobertura;lcov;TextSummary"

# dotCover: run coverage and emit HTML report (no snapshot)
RUN dotCover cover-dotnet \
  --output /app/testresults/dotcover-report.html \
  --reportType HTML \
  -- test tests/SmsService.UnitTests/SmsService.UnitTests.csproj \
         --no-build -c $BUILD_CONFIGURATION \
         --logger "trx;LogFileName=dotcover_results.trx" \
         --results-directory /app/testresults/dotcover

# Run dotCover (2025.2.*) pass — wraps dotnet test on the same csproj
#RUN dotCover cover /TargetExecutable=dotnet /TargetArguments="test tests/SmsService.UnitTests/SmsService.UnitTests.csproj --no-build -c $BUILD_CONFIGURATION --logger trx;LogFileName=dotcover_results.trx --results-directory /app/testresults/dotcover" /Output=/app/testresults/dotcover.dcvr
#RUN dotCover report /Source=/app/testresults/dotcover.dcvr /ReportType=HTML /Output=/app/testresults/dotcover-report.html

# -------------------- CLEANUP --------------------
FROM build AS cleanup
WORKDIR /app
RUN dotnet tool uninstall --global JetBrains.dotCover.CommandLineTools \
    && dotnet tool uninstall --global dotnet-coverage \
    && dotnet tool uninstall --global dotnet-reportgenerator-globaltool \
    && rm -rf /root/.dotnet/tools \
    && rm -rf /root/.nuget/packages 

# ================== SERVE REPORT ==================
FROM python:3.12-alpine AS serve
WORKDIR /srv
COPY --from=testrunner /app/testresults /srv/testresults
EXPOSE 8080
CMD ["python", "-m", "http.server", "8080", "-d", "/srv/testresults"]