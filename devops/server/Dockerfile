FROM nginx:alpine

#Copy main nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

#Copy template configuration files
COPY conf.d/gateway.conf /etc/nginx/templates/gateway.conf.template
COPY conf.d/registry.conf /etc/nginx/templates/registry.conf.template

#Copy custom configuration files
COPY conf.d/ /etc/nginx/conf.d/
COPY certs/ /etc/nginx/certs/

EXPOSE 80 443