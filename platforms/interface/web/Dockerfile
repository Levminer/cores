# --- Build Stage ---
FROM node:22-alpine AS builder
WORKDIR /app

# Copy root package files for workspace
COPY package.json package-lock.json ./

# Copy workspace packages
COPY platforms/interface ./platforms/interface
COPY scripts ./scripts

RUN npm i
RUN npm run build:web

# --- Production Stage ---
FROM node:22-alpine
WORKDIR /app
COPY --from=builder /app/platforms/interface/web/build ./build
EXPOSE 3000
CMD ["node", "build"]
