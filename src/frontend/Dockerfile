# Stage 1: Build the Angular application

# Use official Node.js 16 as the base image
FROM node:16 as build

# Set working directory in the Docker container
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install npm packages
RUN npm install

# Copy the rest of the application to the working directory
COPY . .

# Generate the build of the application
RUN npm run build --prod

# Stage 2: Serve the application with Nginx

# Use official Nginx image as the base image
FROM nginx:alpine

# Remove default Nginx website
RUN rm -rf /usr/share/nginx/html/*

# Copy the build output to replace the default Nginx website
COPY --from=build /app/dist /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Run Nginx without daemonizing
CMD ["nginx", "-g", "daemon off;"]
