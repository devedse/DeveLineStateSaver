# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /source

# caches restore result by copying csproj file separately
#COPY /NuGet.config /source/
COPY /DeveLineStateSaver/*.csproj /source/DeveLineStateSaver/
COPY /DeveLineStateSaver.ConsoleApp/*.csproj /source/DeveLineStateSaver.ConsoleApp/
COPY /DeveLineStateSaver.Tests/*.csproj /source/DeveLineStateSaver.Tests/
COPY /DeveLineStateSaver.sln /source/
RUN ls
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet build --configuration Release
RUN dotnet test --configuration Release ./DeveLineStateSaver.Tests/DeveLineStateSaver.Tests.csproj
RUN dotnet publish ./DeveLineStateSaver.ConsoleApp/DeveLineStateSaver.ConsoleApp.csproj --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine3.9
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "DeveLineStateSaver.ConsoleApp.dll"]