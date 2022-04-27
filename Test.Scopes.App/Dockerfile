ARG SDK_VERSION="6.0.100-alpine3.14"
ARG BASE_ADRESS="mcr.microsoft.com/dotnet"

FROM $BASE_ADRESS/sdk:$SDK_VERSION AS build

WORKDIR /src

COPY ./Test.Scopes.App/*.csproj ./Test.Scopes.App/
RUN dotnet restore -v m ./Test.Scopes.App

COPY ./Test.Scopes.App/. ./Test.Scopes.App/

WORKDIR /Test.Scopes.App

RUN dotnet build -c Release --no-restore -v m -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /app/publish

FROM nginx:alpine AS final
COPY --from=publish /app/publish/wwwroot ./usr/share/nginx/html