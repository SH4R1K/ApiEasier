FROM node:current-alpine3.20 AS build
WORKDIR /front
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --prod

FROM nginx:stable-perl
COPY --from=build /front/dist/ApiEasier/browser /usr/share/nginx/html

EXPOSE 80
CMD [ "nginx", "-g", "daemon off;" ]
