FROM node:lts-alpine AS build
WORKDIR /pass-client
COPY package*.json ./

RUN npm install --legacy-peer-deps
COPY . .
RUN npm run build:prod

CMD ["sh", "-c", "cp -r dist/pass-client/browser/* /output/ && echo 'Client artifacts copied to shared volume.'"]