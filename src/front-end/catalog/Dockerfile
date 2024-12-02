FROM node:alpine AS builder
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

FROM nginx:alpine AS prod
WORKDIR /usr/share/nginx/html
COPY --from=builder /app/build ./
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]