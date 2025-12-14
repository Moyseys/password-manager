FROM nginx:alpine

COPY nginx.conf /etc/nginx/nginx.conf
COPY conf.d/ /etc/nginx/conf.d/
COPY certs/ /etc/nginx/certs/

EXPOSE 80 443