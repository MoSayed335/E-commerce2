# E-commerce2

A full-featured e-commerce REST API built with **ASP.NET Core 9** and **Entity Framework Core**, providing storefront, customer, and admin capabilities for managing products, orders, carts, coupons, reviews, shipping, and more.

## Tech Stack

- **Framework:** ASP.NET Core 9.0 (Web API)
- **ORM:** Entity Framework Core 9.0 (SQL Server)
- **Auth:** ASP.NET Core Identity + JWT Bearer authentication
- **API Docs:** Scalar (OpenAPI UI)
- **Architecture:** Repository + Unit of Work pattern, layered Services/Controllers/DTOs

## Features

### Storefront (Public / Customer-facing)
- Browse products by category, view product details and variants (color/size)
- Category listing, banners, and lookups (colors, sizes, governorates)
- Shopping cart management
- Checkout with coupon support and shipping calculation
- Order tracking by order number + email
- Product reviews (submit & view)
- Favorites (wishlist)
- User profile & address book

### Admin Panel API
- Product, category, color, size (attribute) management
- Order management (status updates, cancellation, admin notes)
- Coupon management with campaign/redemption tracking
- Banner management (homepage promotions)
- Review moderation (approve/reject, pin, live urgency counter)
- Customer list & summary
- Shipping settings (free-shipping threshold, per-governorate rates)
- Image/file uploads

### Auth & Identity
- User registration/login with JWT issuance
- Role-based authorization (`Admin`, `Customer`)
- Seeded default admin account on first run

## Project Structure

```
E-commerce2/
├── Controllers/
│   ├── AuthController.cs            # Register / Login
│   ├── ProfileController.cs         # User profile & addresses
│   ├── Admin/                       # Admin-only endpoints (Banners, Categories, Colors,
│   │                                 # Coupons, Customers, Orders, Products, Reviews,
│   │                                 # Shipping, Sizes, Uploads)
│   ├── Customer/                    # Authenticated customer endpoints (Orders, Products)
│   └── Storefront/                  # Public storefront endpoints (Banners, Cart, Categories,
│                                     # Checkout, Coupons, Favorites, Lookups, Orders, Reviews)
├── DataAccess/
│   ├── AppDbContext.cs              # EF Core DbContext (extends IdentityDbContext<User>)
│   ├── DataSeeder.cs                # Seeds roles, admin user, default governorate & settings
│   └── Configurations/              # Fluent API entity configurations
├── DTOs/Responses/                  # Request/response DTOs (records) per domain area
├── Migrations/                      # EF Core migrations
├── Models/                          # Domain entities (Product, Order, Cart, Coupon, Banner,
│                                     # Review, Favorite, Governorate, User, etc.) + Enums
├── Repositories/
│   ├── GenericRepository.cs / IGenericRepository.cs
│   ├── SpecificRepositories.cs      # Per-entity repositories
│   ├── UnitOfWork.cs
│   └── Interfaces/IRepositories.cs
├── Services/
│   ├── *.cs                         # Business logic per domain (Product, Order, Cart,
│   │                                 # Coupon, Category, Auth, Profile, Review, Shipping, etc.)
│   └── Interfaces/IServices.cs
├── Utilities/
│   ├── Result.cs                    # Generic Result<T> wrapper for service outcomes
│   ├── PaginatedList.cs             # Pagination helper
│   ├── IGenericRepository.cs
│   └── SD.cs                        # Static/shared definitions
└── Program.cs                       # App startup, DI registration, middleware pipeline
```

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB is used by default) or another SQL Server instance

### Setup

1. **Clone the repository**
   ```bash
   git clone <repo-url>
   cd E-commerce2
   ```

2. **Configure the database connection**

   Update the `ConnectionStrings:DefaultConnection` value in `appsettings.json` / `appsettings.Development.json` if you're not using LocalDB:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=(localdb)\\ProjectModels;Initial Catalog=Ecommerce2;..."
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Explore the API**

   In development mode, an OpenAPI reference UI is available via Scalar at the app's root/`/scalar` route once running.

### Default Admin Account

On first run, `DataSeeder` creates:
- **Email:** `admin@admin.com`
- **Password:** `Admin123!`
- **Role:** `Admin`

> ⚠️ Change this password before deploying to any shared or production environment.

## Authentication

The API uses JWT Bearer tokens. Configure the JWT section in `appsettings.json`:

```json
"Jwt": {
  "Key": "<your-secret-key-at-least-32-bytes>",
  "Issuer": "ECommerce2API",
  "Audience": "ECommerce2FrontEnd",
  "ExpireDays": 7
}
```

1. `POST /api/Auth/register` — create an account
2. `POST /api/Auth/login` — obtain a JWT
3. Include the token as `Authorization: Bearer <token>` on subsequent requests to protected endpoints

## Key API Areas

| Area | Base Route | Notes |
|---|---|---|
| Auth | `/api/Auth` | Register, Login |
| Profile | `/api/Profile` | Requires auth |
| Storefront | `/api/storefront/*` | Public browsing + authenticated cart/checkout/favorites |
| Customer Orders | `/api/Orders` | Requires auth |
| Admin | `/api/admin/*` | Requires `Admin` role |

> ⚠️ **Note:** `appsettings.json` in this repository contains a placeholder JWT signing key committed to source control. Rotate this secret (e.g. via user secrets or environment variables) before any real deployment.

## Architecture Notes

- **Result Pattern:** Services return a `Result<T>` (see `Utilities/Result.cs`) to represent success/failure without throwing exceptions for expected business errors.
- **Pagination:** List endpoints commonly return a `PaginatedList<T>` for consistent paging metadata.
- **Repository + Unit of Work:** Data access is abstracted through `IGenericRepository<T>` / specific repositories, coordinated by `IUnitOfWork` for transactional `SaveChanges`.
- **Soft Deletes:** Products use a query filter (`IsDeleted`) so removed products are excluded from queries without losing historical data (e.g., for past orders).

## License

Add your license of choice here.
