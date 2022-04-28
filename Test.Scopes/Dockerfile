FROM mcr.microsoft.com/dotnet/sdk:6.0.100-alpine3.14 AS build-env

WORKDIR /src

COPY ./Test.Scopes/*.csproj ./Test.Scopes/

RUN dotnet restore -v m ./Test.Scopes/

RUN dotnet build -c Release --no-restore -v m -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.Scopes.dll"]