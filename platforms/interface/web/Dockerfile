# --- Build Stage ---
FROM node:22-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build:docker

# --- Production Stage ---
FROM node:22-alpine
WORKDIR /app
COPY --from=builder /app/build ./build
COPY package*.json ./
RUN npm ci --omit=dev
EXPOSE 3000
CMD ["npx", "serve", "build"]
