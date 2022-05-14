FROM cypress/base:12.18.2

WORKDIR /app

COPY ./Test.Scopes.E2E/cypress ./Test.Scopes.E2E/cypress/
COPY ./Test.Scopes.E2E/*.json ./Test.Scopes.E2E/

WORKDIR ./Test.Scopes.E2E

RUN npm ci
RUN npx cypress verify
