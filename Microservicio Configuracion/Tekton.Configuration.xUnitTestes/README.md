# ejecutar docker compose
docker-compose up -d

# config error max_map_count
wsl -d docker-desktop sysctl -w vm.max_map_count=262144

# sonarqube 
user 		: admin
password 	: admin

# subir backend Tekton.Configuration.WebApi (KEY)
dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"Tekton.Configuration.WebApi" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="{KEY}"
dotnet build
dotnet sonarscanner end /d:sonar.login="{KEY}"


# subir Test Tekton.Configuration.WebApi JSON (KEY)
dotnet tool install -g coverlet.console
dotnet sonarscanner begin /k:"Tekton.Configuration.WebApi" /d:sonar.login="{KEY}" /d:sonar.cs.opencover.reportsPaths=coverage.json
dotnet build --no-incremental
coverlet .\bin\Debug\net6.0\Tekton.Configuration.xUnitTests.dll --target "dotnet" --targetargs "test --no-build" -f=opencover -o="coverage.json"
dotnet sonarscanner end /d:sonar.login="{KEY}"


# subir Test Tekton.Configuration.WebApi XML (sqp_a451108a27a95deeabeb3fc57f33998e9c07a8d8)
dotnet tool install -g coverlet.console
dotnet sonarscanner begin /k:"Tekton.Configuration.WebApi" /d:sonar.login="{KEY}" /d:sonar.cs.opencover.reportsPaths=coverage.xml
dotnet build --no-incremental
coverlet .\bin\Debug\net6.0\Tekton.Configuration.xUnitTests.dll --target "dotnet"  --targetargs "test --no-build" -f=opencover -o="coverage.xml"
dotnet sonarscanner end /d:sonar.login="{KEY}"