# --- Build Stage ---
FROM --platform=$BUILDPLATFORM node:26-alpine AS builder
WORKDIR /app

# Copy root package files for workspace
COPY package.json package-lock.json ./

# Copy workspace packages
COPY platforms/interface ./platforms/interface
COPY scripts ./scripts

RUN npm ci
RUN npm run build:web

# --- Production Stage ---
FROM node:26-alpine
WORKDIR /app
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/platforms/interface/web/build ./build
EXPOSE 3000
CMD ["node", "build"]
