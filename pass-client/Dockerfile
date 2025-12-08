FROM node:20-alpine AS build
WORKDIR /pass-client
COPY package*.json ./

RUN npm ci --no-audit --silent
COPY . .
RUN npm run build
