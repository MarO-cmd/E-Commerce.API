# Store System

## Overview
Store System is a comprehensive e-commerce backend API built with ASP.NET Core Web API. This project implements modern software architecture patterns and technologies.

## Features
- **Product Browsing**: View all store products
- **Filtering**: Filter by product type or brand
- **Shopping Basket**: Create and manage baskets
- **Checkout System**: Complete orders with payment options
- **Online Payments**: Stripe integration
- **User Authentication**: JWT-based auth with ASP.NET Core Identity

## Technical Implementation
- **Architecture**: Onion (Clean) Architecture
- **Design Patterns**:
  - Generic Repository
  - Unit of Work
  - Specification
- **Performance**:
  - Redis caching
  - Response caching
  - Pagination
- **Security**:
  - JWT Authentication
  - ASP.NET Core Identity
- **Error Handling**: Custom middleware

## Installation

### Prerequisites
- .NET 6.0 SDK+
- SQL Server
- Redis server
- Stripe account

### Steps
1. Clone repository:
```bash
git clone https://github.com/MarO-cmd/Store.API.git
cd StoreSystem
Update appsettings.json:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=StoreSystem;Trusted_Connection=True;",
  "Redis": "localhost:6379"
},
"StripeSettings": {
  "PublishableKey": "your_key",
  "SecretKey": "your_key"
}
Apply migrations:

bash
dotnet ef database update
Run:

bash
dotnet run
API Endpoints
Accounts
POST /api/Accounts/login - Login

POST /api/Accounts/register - Register

GET /api/Accounts/getcurrentuser - Current user info

Baskets
GET /api/Baskets - Get basket

POST /api/Baskets - Update basket

DELETE /api/Baskets - Delete basket

Orders
GET /api/Orders - Get orders

POST /api/Orders - Create order

Products
GET /api/Products - Get all products

GET /api/Products/{id} - Get product by ID

Error Handling
Custom middleware handles all exceptions with appropriate HTTP status codes.

Contact
GitHub: MarO-cmd

Email: maroasd33@gmail.com

