# sms-service

A minimal SMS sending API supporting Twilio and İleti Merkezi. Receives HTTP POST requests and sends SMS via the configured provider.

## Run

```bash
dotnet restore
dotnet run --project src/SmsService.API/SmsService.API.csproj
```

## API

POST `/api/v1/sms-send`

```json
{
  "PhoneNumber": "+15551234567",
  "Message": "Hello from sms-service!"
}
```

## Config

- `Twilio.Enabled`: true/false
- `IletiMerkezi.Enabled`: true/false

## Deployment

Existing `k8s-manifests/` and `gitlab-manifests/` have been preserved. Update image name and environment variables accordingly.

---
## 🗂️ Project Structure

Below is the updated folder structure of the project:

```text
sms-service/
├── etc/                                # Static files (e.g. diagrams, json samples, images)
├── NotificationService.sln             # .NET solution file
├── src/                                # Source code for the application
│   ├── SmsService.API/                 # ASP.NET Core Web API project
│   ├── SmsService.Application/         # Application layer (CQRS, commands, queries)
│   └── SmsService.Infrastructure/      # Infrastructure implementations (FCM, SMS providers)
├── tests/                              # Unit and integration tests
│   └── SmsService.UnitTests/           # Unit tests project
├── gitlab-manifests/                   # GitLab CI templates for environment-specific jobs
│   ├── api/
│   │   ├── dev/                        # CI config for API in development
│   │   │   └── .gitlab-ci.yml
│   │   ├── staging/                    # CI config for API in staging
│   │   │   └── .gitlab-ci.yml
│   │   └── prod/                       # CI config for API in production
│   └──     └── .gitlab-ci.yml
├── k8s-manifests/                      # Kubernetes deployment files
│   ├── api/
│   │   ├── .env                        # Base environment variables for API
│   │   ├── deployment.yaml             # Deployment configuration for API
│   │   ├── config.yaml                 # ConfigMap for API
│   │   ├── hpa.yaml                    # Horizontal Pod Autoscaler for API
│   └── └── service.yaml                # ClusterIP or LoadBalancer service for API
├── README.md                           # Project documentation
└── .gitlab-ci.yml                      # Root GitLab pipeline (entry point)
```

---

## 🐳 Docker
> Once running, open [http://localhost:8080](http://localhost:8080) to browse coverage reports directly.
```bash
docker build -f tests/Dockerfile -t smssvc-coveragereport .
docker run -d -p 8080:8080 --name smssvc-coveragereport smssvc-coveragereport
```

Stop & clean:
```bash
docker rm -f smssvc-coveragereport
```

Optional (build only the test-runner stage and copy artifacts locally):
```bash
docker build -f tests/Dockerfile --target testrunner -t smssvc-tests .
cid=$(docker create smssvc-tests)
docker cp "$cid":/app/testresults ./testresults
docker rm "$cid"
open ./testresults/coverage-report/index.html
```

![CoverageReports](etc/coveragereport-docker.png?raw=true)


---

## 🧪 Unit Tests

```bash
# Run all unit tests (excluding Integration)
dotnet test SmsService.sln -c Release --filter "Category!=Integration"

# Or run directly
dotnet test ./tests/SmsService.UnitTests/SmsService.UnitTests.csproj -c Release
```

---

## 📊 Test Coverage

### Option 1: coverlet.collector

> ℹ️ Info:
> Requires the `coverlet.collector` NuGet package.

```bash
dotnet test ./tests/SmsService.UnitTests/SmsService.UnitTests.csproj -c Release \
  --collect:"XPlat Code Coverage" \
  --results-directory tests/SmsService.UnitTests/TestResults/coverage && \
  find tests/SmsService.UnitTests/TestResults/coverage -name "coverage.cobertura.xml" \
  -exec sh -c 'cp "$1" tests/SmsService.UnitTests/TestResults/coverage/coverage.cobertura.xml && rm -rf "$(dirname "$1")"' _ {} \;
```

### Option 2: coverlet.msbuild

> ℹ️ Info:
> Requires the `coverlet.msbuild` NuGet package.

```bash
dotnet test ./tests/SmsService.UnitTests/SmsService.UnitTests.csproj -c Release \
  /p:CollectCoverage=true \
  /p:CoverletOutput=TestResults/coverage/ \
  /p:CoverletOutputFormat=cobertura
```

### Optional Filters

```bash
dotnet test ./tests/SmsService.UnitTests/SmsService.UnitTests.csproj -c Release \
  /p:CollectCoverage=true \
  /p:CoverletOutput=TestResults/coverage/ \
  /p:CoverletOutputFormat=cobertura \
  '/p:Include="[SmsService.Application]*,[SmsService.API]*"' \
  '/p:Exclude="[*.Tests]*,[SmsService.IntegrationTests]*,[SmsService.UnitTests]*"'
```

![CoverageMSBuild](etc/msbuild-coverage-result.png?raw=true)

---

## 📝 Coverage Formats

| Format     | Description                                                        | Use Case                        |
|------------|--------------------------------------------------------------------|---------------------------------|
| cobertura  | XML format for GitLab, Azure DevOps, Jenkins                      | ✅ Recommended for pipelines     |
| opencover  | Compatible with Rider, Visual Studio, ReportGenerator             | Good for local usage            |
| lcov       | Used by SonarQube, web-based visualizers                          | Optional                        |
| json       | Raw data format                                                    | Rarely used                     |

---

## 🔧 Config via `.csproj`

```xml
<PropertyGroup>
  <CollectCoverage>true</CollectCoverage>
  <CoverletOutput>TestResults/coverage/</CoverletOutput>
  <CoverletOutputFormat>cobertura</CoverletOutputFormat>
  <Include>[SmsService.Application]*,[SmsService.API]*</Include>
  <Exclude>[*.Tests]*,[SmsService.IntegrationTests]*,[SmsService.UnitTests]*</Exclude>
</PropertyGroup>
```

### Option 3: dotCover
> ℹ️ Info:
> ref: https://www.jetbrains.com/help/dotcover/Coverage-Analysis-with-Command-Line-Tool.html#install-dotcover-command-line-tool
> You’ll need the dotCover Command Line Tools to generate report and convert coverage results (like dvcr) into reports.
Install it once using:

```bash
dotnet tool install --global JetBrains.dotCover.CommandLineTools
```
ref: https://www.jetbrains.com/help/dotcover/dotCover__Console_Runner_Commands.html
```bash
dotCover cover \
  --target-executable "/usr/local/share/dotnet/dotnet" \
  --target-arguments "test /Users/gulizay/Projects/GithubProjects/sms-service/tests/SmsService.UnitTests/SmsService.UnitTests.csproj --no-build --configuration Release" \
  --snapshot-output /Users/gulizay/Projects/GithubProjects/sms-service/tests/SmsService.UnitTests/snapshot.dcvr 
```

> ⚠️ Note:
> Report not working!
> Error:
> [JetBrains dotCover] Unhandled exception: Snapshot container is not initialized
```sh
dotCover cover \
  --target-executable "/usr/local/share/dotnet/dotnet" \
  --target-arguments "test /Users/gulizay/Projects/GithubProjects/sms-service/tests/SmsService.UnitTests/SmsService.UnitTests.csproj --no-build --configuration Release" \
  --xml-report-output /Users/gulizay/Projects/GithubProjects/sms-service/tests/SmsService.UnitTests/coverage.xml
```

> ⚠️ Note:
> Report not working!
> Error:
> [JetBrains dotCover] Unhandled exception: Object reference not set to an instance of an object.
```sh
dotCover report \
  --snapshot-source /Users/gulizay/Projects/GithubProjects/sms-service/tests/SmsService.UnitTests/snapshot.dcvr \
  --xml-report-output /Users/gulizay/Projects/GithubProjects/sms-service/tests/SmsService.UnitTests/coverage.xml
```

## 🧪 Generate HTML Coverage Report with ReportGenerator

> ℹ️ Info:
> You’ll need the ReportGenerator .NET global tool to convert coverage results (like Cobertura) into HTML reports.
Install it once using:

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

```bash
reportgenerator \
    -reports:tests/SmsService.UnitTests/TestResults/coverage/coverage.cobertura.xml \
    -targetdir:tests/SmsService.UnitTests/TestResults/html \
    -reporttypes:Html
```
![ReportGenerator](etc/reportgenerator-html.png?raw=true)