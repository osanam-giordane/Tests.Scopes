ARG SDK_VERSION="6.0.100-alpine3.14"
ARG BASE_ADRESS="mcr.microsoft.com/dotnet"

FROM $BASE_ADRESS/sdk:$SDK_VERSION AS build

WORKDIR /src

EXPOSE 8000

COPY ./Test.Scopes.App/*.csproj ./Test.Scopes.App/

RUN dotnet restore -v m ./Test.Scopes.App

COPY ./Test.Scopes.App/. ./Test.Scopes.App/

WORKDIR /src/Test.Scopes.App
RUN dotnet build -c Release --no-restore -v m -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /publish

FROM nginx:alpine AS final
COPY --from=publish /publish/wwwroot ./usr/share/nginx/html