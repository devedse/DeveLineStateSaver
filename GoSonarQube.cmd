SonarScanner.MSBuild.exe begin /k:"DeveLineStateSaver" /d:sonar.organization="devedse-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="6e589700e7cec3bcec784e50b5b91c5ecaf8326a"
dotnet build -c Release
SonarScanner.MSBuild.exe end /d:sonar.login="6e589700e7cec3bcec784e50b5b91c5ecaf8326a"