# Babs Kitap Evi (Book Store API)

A comprehensive book store management system built with .NET 9 and Angular. This project provides a complete solution for managing books, categories, publishers, users, orders, and shopping cart functionality.

## 🚀 Technologies

### Backend

- **.NET 9 Web API** - RESTful API development
- **Entity Framework Core** - ORM for database operations
- **Microsoft SQL Server** - Database management
- **ASP.NET Core Identity** - User authentication and authorization
- **JWT Authentication** - Secure token-based authentication
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation

### Frontend (Companion Project)

- **Angular 18** - TypeScript-based frontend framework
- **Bootstrap** - Responsive UI framework
- **RxJS** - Reactive programming

🔗 **Frontend Repository:** [BabsKitapEviWeb](https://github.com/berkaychi/BabsKitapEviWeb)

## 📚 Features

- **User Management** - Registration, authentication, and role-based access control
- **Book Management** - Complete CRUD operations for books with categories and publishers
- **Shopping Cart** - Add/remove items, quantity management
- **Order Management** - Order creation, tracking, and history
- **Category & Publisher Management** - Organize books by categories and publishers
- **Address Management** - User address management for orders
- **Stock Management** - Inventory tracking and management
- **Image Upload** - Book cover image management
- **Search & Filtering** - Advanced book search and filtering capabilities

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── Entities/           # Domain entities and models
├── DataAccess/         # Data access layer, DbContext, migrations
├── Business/           # Business logic, services, and interfaces
├── Common/             # Shared DTOs and utilities
└── WebAPI/             # API controllers and configuration
```

## 🛠️ Getting Started

### Prerequisites

- .NET 9 SDK or later
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/berkaychi/BabsKitapEvi.git
   cd BabsKitapEvi
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Update database connection string**

   - Edit `WebAPI/appsettings.json`
   - Update the `DefaultConnection` string to match your SQL Server instance

4. **Apply database migrations**

   ```bash
   dotnet ef database update --project DataAccess --startup-project WebAPI
   ```

5. **Run the application**

   ```bash
   dotnet run --project WebAPI
   ```

6. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - API Base URL: `https://localhost:5001/api`

## 📋 API Endpoints

### Authentication

- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh JWT token

### Books

- `GET /api/books` - Get all books with filtering and pagination
- `GET /api/books/{id}` - Get book by ID
- `POST /api/books` - Create new book (Admin)
- `PUT /api/books/{id}` - Update book (Admin)
- `DELETE /api/books/{id}` - Delete book (Admin)

### Categories

- `GET /api/categories` - Get all categories
- `POST /api/categories` - Create category (Admin)
- `PUT /api/categories/{id}` - Update category (Admin)
- `DELETE /api/categories/{id}` - Delete category (Admin)

### Cart

- `GET /api/carts` - Get user's cart
- `POST /api/carts` - Add item to cart
- `PUT /api/carts/{id}` - Update cart item
- `DELETE /api/carts/{id}` - Remove item from cart

### Orders

- `GET /api/orders` - Get user's orders
- `POST /api/orders` - Create new order
- `GET /api/orders/{id}` - Get order details

## 🔧 Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "BabsKitapEvi",
    "Audience": "BabsKitapEviUsers",
    "ExpireMinutes": 60
  }
}
```

### Database Connection

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)mssqllocaldb;Database=BabsKitapEviDB;Trusted_Connection=true;"
  }
}
```

## 🧪 Development Commands

```bash
# Build the solution
dotnet build

# Run tests (if available)
dotnet test

# Create new migration
dotnet ef migrations add <MigrationName> --project DataAccess --startup-project WebAPI

# Update database
dotnet ef database update --project DataAccess --startup-project WebAPI

# Drop database (development only)
dotnet ef database drop --project DataAccess --startup-project WebAPI
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with modern .NET technologies
- Follows industry best practices and design patterns
- Designed for scalability and maintainability
