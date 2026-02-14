FROM nginx:alpine

#Copy main nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

#Copy template configuration files
COPY conf.d-templates/ /etc/nginx/templates/

#Copy custom configuration files
COPY conf.d/ /etc/nginx/conf.d/

EXPOSE 80 443