FROM nginx:1.17-alpine

# RUN rm -f /etc/nginx/conf.d/default.conf

COPY .docker/proxy/nginx.conf /etc/nginx/nginx.conf
COPY .docker/proxy/sfira.conf /etc/nginx/sites-available/sfira.conf

CMD ["nginx", "-g", "daemon off;"]
