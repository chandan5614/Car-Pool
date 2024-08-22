# UniPool

UniPool is a car-pooling application designed to facilitate transportation for international students. The platform allows students to share rides, manage schedules, and handle payments securely.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [Testing](#testing)

## Features

- **User Authentication**: Secure JWT-based authentication.
- **Ride Management**: Users can offer or join rides.
- **Payment Integration**: Secure payment processing for ride-sharing fees.
- **Rating System**: Rate drivers and passengers to ensure a safe experience.
- **Schedule Management**: Plan and manage ride schedules.

## Tech Stack

- **Backend**: ASP.NET Core
- **Database**: Azure Cosmos DB
- **Authentication**: JWT (JSON Web Tokens)
- **Environment Management**: DotNetEnv

## Installation

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Azure Cosmos DB Account](https://azure.microsoft.com/en-us/services/cosmos-db/)

### Steps

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/UniPool.git
    cd UniPool
    ```

2. Install dependencies:

    ```bash
    dotnet restore
    ```

3. Set up environment variables by creating a `.env` file in the root directory:

    ```plaintext
    JWT_SECRET=your_jwt_secret
    COSMOSDB_CONNECTION_STRING=your_cosmosdb_connection_string
    DATABASE_NAME=UniPoolDB
    USER_CONTAINER_NAME=Users
    BOOKING_CONTAINER_NAME=Bookings
    RIDE_CONTAINER_NAME=Rides
    PAYMENT_CONTAINER_NAME=Payments
    RATING_CONTAINER_NAME=Ratings
    SCHEDULE_CONTAINER_NAME=Schedules
    ```

## Configuration

### CORS Configuration

Ensure CORS is properly configured to allow your frontend or API consumers to interact with the backend. Modify the CORS settings in `Program.cs` as needed.

### JWT Authentication

Set the `JWT_SECRET` environment variable to a strong, secure key. This will be used for signing JWT tokens.

### Cosmos DB

Make sure your Azure Cosmos DB instance is correctly set up and connected via the `COSMOSDB_CONNECTION_STRING` in the `.env` file.

## Running the Application

1. Run the application:

    ```bash
    dotnet run
    ```

2. Access the Swagger UI for API documentation and testing:

    - Development: `https://localhost:7026/swagger`
    - Production: Replace with your production URL.

## Testing

You can test JWT authentication via the Swagger UI. Make sure to generate a token and use it to access the secured endpoints.

To test the API:

1. Generate a JWT token using your login endpoint.
2. Use the token in the Swagger UI or your API testing tool (like Postman) to access the protected routes.

