# Store System

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-6.0-blue.svg)](https://dotnet.microsoft.com/en-us/apps/aspnet)
[![C#](https://img.shields.io/badge/C%23-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-6.0-green.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![Redis](https://img.shields.io/badge/Redis-Cache-red.svg)](https://redis.io/)
[![JWT](https://img.shields.io/badge/JWT-Auth-orange.svg)](https://jwt.io/)

## Overview

Store System is a comprehensive e-commerce backend API built with ASP.NET Core Web API. This project was developed as part of my learning journey, implementing modern software architecture patterns and technologies.

## Features

* **Product Browsing**: Users can view all products available in the store
* **Filtering**: Get products by type or brand
* **Shopping Basket**: Create and manage shopping baskets
* **Checkout System**: Complete orders with different payment options
* **Online Payments**: Integrated with Stripe for secure online payments
* **User Authentication**: JWT-based authentication using ASP.NET Core Identity

## Technical Implementation

* **Architecture**: Onion (Clean) Architecture
* **Design Patterns**:
   * Generic Repository Pattern
   * Unit of Work Pattern
   * Specification Pattern
* **Performance Optimization**:
   * Redis caching for basket storage
   * Response caching for frequently requested endpoints
   * Pagination for large data sets
* **Security**:
   * JWT Authentication
   * ASP.NET Core Identity
* **Error Handling**: Custom middleware for global exception handling

## Installation

### Prerequisites
* .NET 6.0 SDK or later
* SQL Server (or alternative compatible database)
* Redis server
* Stripe account for payment processing

### Steps
1. **Clone the repository**

```
git clone https://github.com/MarO-cmd/Store.API.git
cd StoreSystem
```

2. **Update the connection strings**
In `appsettings.json`, update the connection strings for your database and Redis:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=StoreSystem;Trusted_Connection=True;MultipleActiveResultSets=true",
  "Redis": "localhost:6379"
}
```

3. **Set up Stripe API keys**
In `appsettings.json`, add your Stripe API keys:

```json
"StripeSettings": {
  "PublishableKey": "your_publishable_key",
  "SecretKey": "your_secret_key"
}
```

4. **Apply database migrations**

```
dotnet ef database update
```

5. **Run the application**

```
dotnet run
```

## API Endpoints

### Accounts
* `POST /api/Accounts/login` - User login
* `POST /api/Accounts/register` - User registration
* `GET /api/Accounts/getcurrentuser` - Get current user information
* `GET /api/Accounts/getaddress` - Get user's address
* `PUT /api/Accounts/updateaddress` - Update user's address

### Baskets
* `GET /api/Baskets` - Get the current user's basket
* `POST /api/Baskets` - Create or update a basket
* `DELETE /api/Baskets` - Delete a basket

### Orders
* `GET /api/Orders` - Get user's orders
* `GET /api/Orders/{id}` - Get a specific order
* `POST /api/Orders` - Create a new order
* `GET /api/Orders/DeliveryMethods` - Get available delivery methods

### Payments
* `POST /api/Payments/{basketId}` - Process a payment for a specific basket

### Products
* `GET /api/Products` - Get all products (with pagination)
* `GET /api/Products/{id}` - Get a specific product
* `GET /api/Products/brands` - Get all product brands
* `GET /api/Products/types` - Get all product types

## Error Handling

The application uses a custom exception middleware that catches all exceptions and returns appropriate HTTP status codes and error messages.

## Acknowledgments

This project was developed as part of my learning journey in ASP.NET Core and modern web API development.

## Contact

- GitHub: [MarO-cmd](https://github.com/MarO-cmd)
- LinkedIn: [Marcellino Adel](https://www.linkedin.com/in/marcellino-adel-752b17235/)
- Email: maroasd33@gmail.com
