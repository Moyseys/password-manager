FROM node:lts-alpine AS build
WORKDIR /pass-client
COPY package*.json ./

RUN npm i
COPY . .
RUN npm run build:prod
