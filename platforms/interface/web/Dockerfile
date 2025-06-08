# --- Build Stage ---
FROM node:22-alpine AS builder
WORKDIR /app
COPY . .
RUN ls -al
RUN rm -rf node_modules
RUN rm package-lock.json
RUN npm i
RUN npm run build:docker

# --- Production Stage ---
FROM node:22-alpine
WORKDIR /app
COPY --from=builder app/platforms/interface/web/build ./build
EXPOSE 3000
CMD ["npx", "serve", "build"]
