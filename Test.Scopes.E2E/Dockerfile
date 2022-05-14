FROM cypress/base:14.17.6-slim

WORKDIR /app

COPY ./Test.Scopes.E2E/cypress ./Test.Scopes.E2E/cypress/
COPY ./Test.Scopes.E2E/*.json ./Test.Scopes.E2E/

WORKDIR ./Test.Scopes.E2E

RUN npm ci
RUN npx cypress verify
