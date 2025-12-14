# Production Grade Web API

# Project Overview

Production Grade Web API is an **enterprise-ready ASP.NET Core 8 Web API** designed with clean architecture principles and production-level standards. This comprehensive solution demonstrates best practices in API development, including robust authentication, data integrity, concurrent transaction management, and comprehensive error handling. Built with a focus on scalability, maintainability, and security, this API provides a complete product catalog and order management system with advanced features like shopping carts, stock management, and JWT-based authentication.

# Key Features

#  Authentication & Authorization
- **JWT-based Authentication** with secure token generation and validation
- **User Registration** with password hashing using BCrypt
- **User Login** with automatic token expiration and refresh capability
- **Claims-based Identity** for secure user context management
- **Role-based Authorization** with [Authorize] attributes on protected endpoints

#  Product Catalog Management
- **CRUD Operations** for products with auto-generated SKUs
- **Product Categorization** with user-specific category isolation
- **Product Images/Pictures** support with multiple images per product
- **Product Status Tracking** (Active, Inactive, Discontinued, OutOfStock)
- **Product Search** by SKU, category, and other filters with pagination

#  Shopping Cart System
- **Dynamic Shopping Cart** with real-time stock validation
- **Cart Item Management** (add, update, remove items)
- **Price Snapshots** capturing product prices at add time
- **Automatic Calculations** for total price and item count
- **Cart Persistence** across user sessions

#  Order Management
- **Order Placement** with comprehensive stock validation
- **Transactional Integrity** using SERIALIZABLE isolation level
- **Concurrent Order Processing** preventing race conditions and overselling
- **Order Status Tracking** (Pending, Confirmed, Failed, Cancelled)
- **Cart Checkout** converting cart items directly to orders
- **Order Cancellation** with automatic stock restoration

#  Data Integrity & Safety
- **SERIALIZABLE Transaction Isolation** for critical operations
- **Pessimistic Locking** preventing concurrent modification conflicts
- **Soft Delete Pattern** maintaining audit trails without permanent deletion
- **Optimistic Concurrency Control** using RowVersion tokens
- **Global Query Filters** automatically excluding soft-deleted records
- **Check Constraints** enforcing business rules at database level

#  Architecture & Design Patterns
- **Clean Architecture** with clear separation of concerns (Domain, Application, Infrastructure, API)
- **Repository Pattern** for data access abstraction
- **Unit of Work Pattern** managing transactions across multiple repositories
- **Dependency Injection** using Microsoft's built-in DI container
- **SOLID Principles** throughout the codebase
- **Service Layer Pattern** encapsulating business logic

#  Validation & Error Handling
- **FluentValidation** for comprehensive input validation
- **Global Exception Handler Middleware** catching all unhandled exceptions
- **Standardized API Response Format** for consistent error/success responses
- **HTTP Status Codes** properly mapped (200, 201, 400, 401, 404, 409, 500)
- **Clear Error Messages** helping clients understand failures

#  Advanced Features
- **Pagination Support** for large data sets
- **Structured Logging** with Serilog (console and file output)
- **Swagger/OpenAPI Documentation** with JWT authorization scheme
- **CORS Support** for cross-origin requests
- **Database Indexing** optimized for common query patterns
- **Audit Trails** with CreatedBy, UpdatedBy, CreatedAt, UpdatedAt fields

# Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Framework** | ASP.NET Core | 8.0 LTS |
| **ORM** | Entity Framework Core | 10.0.0 |
| **Database** | SQL Server | 2019+ |
| **Authentication** | JWT Bearer | System.IdentityModel.Tokens.Jwt |
| **Validation** | FluentValidation | 11.9.2 |
| **Mapping** | AutoMapper | 13.0.1 |
| **Logging** | Serilog | 4.1.0 |
| **API Documentation** | Swagger/OpenAPI | Swashbuckle.AspNetCore 6.4.8 |
| **Password Hashing** | BCrypt.Net-Core | 1.6.0 |
| **Testing** | xUnit | 2.7.0 |

# Project Structure

```
Production-Grade-Web-API/
??? Domain/                          # Core business entities and interfaces
?   ??? Entities/                    # BaseEntity, Product, Order, Cart, etc.
?   ??? Enums/                       # ProductStatus, OrderStatus
?   ??? Interfaces/                  # IRepository, IUnitOfWork
??? Application/                     # Business logic and DTOs
?   ??? DTOs/                        # Data transfer objects for API
?   ??? Services/                    # CategoryService, ProductService, etc.
?   ??? Validators/                  # FluentValidation rules
?   ??? Interfaces/                  # Service contracts
?   ??? Mappings/                    # AutoMapper configuration
??? Infrastructure/                  # Data access and external services
?   ??? Data/                        # DbContext, Repositories, UnitOfWork
?   ??? Configuration/               # Entity configurations (Fluent API)
?   ??? Services/                    # SKU generation, JWT service
??? API/                             # ASP.NET Core API layer
    ??? Controllers/                 # Auth, Products, Orders, Carts, Categories
    ??? Middleware/                  # Exception handling, logging
    ??? Common/                      # ApiResponse wrapper
    ??? Program.cs                   # DI configuration and middleware setup
```

# API Endpoints

# Authentication
```
POST   /api/v1/auth/register        Register new user
POST   /api/v1/auth/login           Login and get JWT token
GET    /api/v1/auth/me              Get current user profile
PUT    /api/v1/auth/me              Update user profile
POST   /api/v1/auth/logout          Logout user
```

# Categories
```
POST   /api/v1/categories           Create category
GET    /api/v1/categories           List user categories
GET    /api/v1/categories/{id}      Get category by ID
PUT    /api/v1/categories/{id}      Update category
DELETE /api/v1/categories/{id}      Delete category
```

# Products
```
POST   /api/v1/products             Create product (auto-generates SKU)
GET    /api/v1/products             List products (paginated)
GET    /api/v1/products/{id}        Get product by ID
GET    /api/v1/products/sku/{sku}   Get product by SKU
GET    /api/v1/products/category/{categoryId}  List by category
PUT    /api/v1/products/{id}        Update product
DELETE /api/v1/products/{id}        Delete product
```

# Shopping Cart
```
GET    /api/v1/carts/my             Get current user cart
POST   /api/v1/carts/items          Add item to cart
PUT    /api/v1/carts/items/{id}     Update item quantity
DELETE /api/v1/carts/items/{id}     Remove item from cart
DELETE /api/v1/carts/clear          Clear entire cart
```

# Orders
```
POST   /api/v1/orders               Place order (validates stock)
GET    /api/v1/orders/{id}          Get order by ID
GET    /api/v1/orders               List user orders (paginated)
PUT    /api/v1/orders/{id}/cancel   Cancel order (restores stock)
POST   /api/v1/orders/checkout      Checkout cart as order
```

# Getting Started

# Prerequisites
- .NET 8.0 SDK or later
- SQL Server 2019 or later
- Visual Studio 2022 or VS Code

# Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/Production-Grade-Web-API.git
cd Production-Grade-Web-API
```

2. **Configure database connection** in `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(local);Database=ProductionGradeWebApiDb;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;"
}
```

3. **Restore NuGet packages**
```bash
dotnet restore
```

4. **Run database migrations**
```bash
dotnet ef database update --project Production.Grade.WebApi.Infrastructure --startup-project Production.Grade.WebApi.API
```

5. **Run the application**
```bash
dotnet run --project Production.Grade.WebApi.API
```

6. **Access Swagger UI**
```
http://localhost:5124/
```

# Usage Examples

# Register a User
```bash
curl -X POST https://localhost:7219/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "username": "Adeola",
    "fullName": "Adebayo Ola",
    "password": "SecurePass123",
    "confirmPassword": "SecurePass123",
    "phoneNumber": "1234567890"
  }'
```
# Login
```bash
curl -X POST https://localhost:7219/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUsername": "user@example.com",
    "password": "SecurePass123"
  }'
```

# Create Product
```bash
curl -X POST https://localhost:7219/api/v1/products \
  -H "Authorization: Bearer {jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop",
    "description": "High-performance laptop",
    "price": 999.99,
    "quantity": 50,
    "categoryId": "{category_id}",
    "status": 1
  }'
```

# Place Order
```bash
curl -X POST https://localhost:7219/api/v1/orders \
  -H "Authorization: Bearer {jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "orderItems": [
      {
        "productId": "{product_id}",
        "quantity": 2
      }
    ],
    "notes": "Urgent delivery"
  }'
```

# Database Schema Highlights

# Key Tables
- **ApplicationUsers** - User accounts with authentication details
- **Categories** - Product categories (user-scoped)
- **Products** - Product catalog with SKUs and stock
- **Pictures** - Product images with display ordering
- **Carts** - Shopping carts (one per user)
- **CartItems** - Items in shopping carts
- **Orders** - Customer orders with status tracking
- **OrderItems** - Products in orders with price snapshots

# Indexing Strategy
- Primary key indexes on all tables
- Unique indexes: SKU, OrderNumber, Email, Username
- Composite indexes for user-specific queries
- Foreign key indexes for relationship performance
- Filtered indexes excluding soft-deleted records

# Security Features

- **Password Security**: BCrypt hashing with salting
- **JWT Tokens**: Secure token-based authentication
- **HTTPS Enforcement**: SSL/TLS for all communications
- **CORS Configuration**: Controlled cross-origin access
- **Input Validation**: Comprehensive validation on all inputs
- **SQL Injection Prevention**: Parameterized queries via EF Core
- **Soft Deletes**: Audit trails without permanent deletion
- **Claims-based Authorization**: Granular permission control

# Performance Optimizations

- **Async/Await**: Non-blocking operations throughout
- **Database Indexing**: Strategic indexes for common queries
- **Pagination**: Limit result sets for large data
- **Connection Pooling**: Efficient database connections
- **Query Optimization**: Minimal N+1 query problems
- **Concurrency Control**: SERIALIZABLE transactions for critical operations

# Testing

Run unit and integration tests:
```bash
dotnet test Production.Grade.WebApi.Tests
```

# Error Handling

The API uses standardized error responses:

# Success Response
```json
{
  "success": true,
  "data": { /* response data */ },
  "errors": [],
  "statusCode": 200,
  "timestamp": "2025-12-14T08:30:00Z"
}
```

# Error Response
```json
{
  "success": false,
  "data": null,
  "errors": ["Error message"],
  "statusCode": 400,
  "timestamp": "2025-12-14T08:30:00Z"
}
```

# Contributing

This project follows clean code principles and SOLID design patterns. When contributing:
1. Follow existing code style and patterns
2. Write unit tests for new features
3. Update documentation
4. Ensure all tests pass

# License

This project is licensed under the MIT License - see LICENSE file for details.

# Support

For issues, questions, or suggestions, please open an issue on GitHub or contact the development team.

---

**Built with ?? using ASP.NET Core 8 and modern web development best practices**