FROM cypress/base:14

WORKDIR /app

COPY ./cypress ./cypress
COPY ./cypress.json ./cypress.json
COPY ./package.json ./package.json
COPY ./package-lock.json ./package-lock.json

ENV CI=1

RUN npm i && npx cypress run