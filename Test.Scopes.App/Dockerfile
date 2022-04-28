ARG SDK_VERSION="6.0.100-alpine3.14"
ARG BASE_ADRESS="mcr.microsoft.com/dotnet"

FROM $BASE_ADRESS/sdk:$SDK_VERSION AS build

WORKDIR /src

COPY ./Test.Scopes.App/*.csproj ./

RUN dotnet restore 
RUN dotnet build -c Release --no-restore -v m -o /build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /publish

FROM nginx:alpine AS final
COPY --from=publish /publish/wwwroot ./usr/share/nginx/html