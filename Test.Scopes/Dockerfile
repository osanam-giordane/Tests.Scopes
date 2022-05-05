FROM mcr.microsoft.com/dotnet/aspnet:6.0.0-alpine3.14 AS base
WORKDIR /app

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:6.0.100-alpine3.14 AS build

WORKDIR /src

COPY ./Test.Scopes/*.csproj ./Test.Scopes/

RUN dotnet restore -v m ./Test.Scopes/

COPY ./Test.Scopes/. ./Test.Scopes/

WORKDIR /src/Test.Scopes

RUN dotnet build -c Release --no-restore -v m -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.Scopes.dll"]