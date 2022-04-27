FROM mcr.microsoft.com/dotnet/sdk:6.0.100-alpine3.15 AS build-env
WORKDIR /app

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Test.Scopes.dll"]