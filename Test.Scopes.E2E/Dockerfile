FROM cypress/base:14

WORKDIR /app

COPY ./Test.Scopes.E2E/cypress ./Test.Scopes.E2E/
COPY ./Test.Scopes.E2E/cypress.json ./Test.Scopes.E2E/
COPY ./Test.Scopes.E2E/package.json ./Test.Scopes.E2E/
COPY ./Test.Scopes.E2E/package-lock.json ./Test.Scopes.E2E/

WORKDIR ./Test.Scopes.E2E

RUN npm i && npx cypress run