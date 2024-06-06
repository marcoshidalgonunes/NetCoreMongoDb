FROM node:alpine as builder
WORKDIR /app
COPY . .
RUN npm install --force
RUN npm run build

FROM nginx:alpine as prod
WORKDIR /usr/share/nginx/html
COPY --from=builder /app/build ./
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]