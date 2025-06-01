# --- Build Stage ---
FROM node:22-alpine AS builder
WORKDIR /app
COPY package.json ./
RUN npm i
COPY . .
# npm bug
RUN rm -rf node_modules
RUN rm package-lock.json
RUN npm i
RUN npm run build:web

# --- Production Stage ---
FROM node:22-alpine
WORKDIR /app
COPY --from=builder /app/platforms/interface/web/build ./build
COPY package.json ./
RUN npm i --omit=dev
EXPOSE 3000
CMD ["npx", "serve", "build"]
