FROM nginx:alpine

COPY nginx.conf /etc/nginx/nginx.conf
COPY conf.d/ /etc/nginx/conf.d/
COPY certs/ /etc/nginx/certs/
COPY --from=pass-client-build /pass-client/dist/pass-client/browser /usr/share/nginx/html

EXPOSE 80 443