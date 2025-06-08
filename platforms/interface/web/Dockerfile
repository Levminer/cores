# --- Build Stage ---
FROM node:22-alpine AS builder
WORKDIR /app
COPY package.json ./
RUN npm i
COPY . .
RUN npm i 
RUN npm run build:docker

# --- Production Stage ---
FROM node:22-alpine
WORKDIR /app
COPY --from=builder /app/platforms/interface/web/build ./build
COPY package.json ./
RUN npm i
EXPOSE 3000
CMD ["npx", "serve", "build"]
