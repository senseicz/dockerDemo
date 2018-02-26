FROM node:alpine
WORKDIR /app

#npm install
COPY package*.json ./
RUN npm install

COPY . .
ENTRYPOINT ["node", "app.js"]