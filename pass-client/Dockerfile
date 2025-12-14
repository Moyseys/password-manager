FROM node:lts-alpine AS build
WORKDIR /pass-client
COPY package*.json ./

RUN npm install --legacy-peer-deps
COPY . .
RUN npm run build:prod
