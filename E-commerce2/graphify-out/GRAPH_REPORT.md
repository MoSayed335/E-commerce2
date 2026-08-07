# Graph Report - .  (2026-08-08)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 900 nodes · 2208 edges · 49 communities (37 shown, 12 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 153 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `635657b3`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- ECommerce2.DTOs
- Result
- Coupon
- ControllerBase
- IAttributeService
- Task
- Category
- IProductService
- ShippingController
- CartController
- ProfileController
- Task
- Product
- AuthService
- Task
- .GetFavorites
- IRepositories.cs
- http
- IBannerService
- AppDbContext
- .GetCustomers
- PaginatedList
- Color
- GenericRepository
- IGenericRepository
- Banner
- BaseEntity
- E-commerce2.csproj
- .ValidateCoupon
- Favorite
- Review
- .GetBanners
- 20260808081228_Phase3_Final_Models.Designer.cs
- AddShippingEntities
- Migration
- AddAdminNotesToOrder
- AddReviewsTable
- AddFavoritesTable
- AddCreatedAtToUser
- Phase3_Final_Models
- AppDbContextModelSnapshot.cs
- StoreSetting
- SD.cs
- 20260808033532_InitialCreate.Designer.cs
- E_commerce2.Migrations
- 20260808070306_AddShippingEntities.Designer.cs
- 20260808071419_AddReviewsTable.Designer.cs
- 20260808072512_AddFavoritesTable.Designer.cs
- 20260808073809_AddCreatedAtToUser.Designer.cs

## God Nodes (most connected - your core abstractions)
1. `Result` - 96 edges
2. `ECommerce2.DTOs` - 42 edges
3. `ECommerce2.Services.Interfaces` - 42 edges
4. `PaginatedList` - 37 edges
5. `ECommerce2.Models` - 35 edges
6. `AppDbContext` - 32 edges
7. `ECommerce2.Utilities` - 26 edges
8. `GenericRepository` - 24 edges
9. `IGenericRepository` - 22 edges
10. `Order` - 20 edges

## Surprising Connections (you probably didn't know these)
- `LookupsController` --references--> `IGovernorateRepository`  [EXTRACTED]
  Controllers/Storefront/LookupsController.cs → Repositories/Interfaces/IRepositories.cs
- `AppDbContext` --references--> `Banner`  [EXTRACTED]
  DataAccess/AppDbContext.cs → Models/Banner.cs
- `AppDbContext` --references--> `BannerCategory`  [EXTRACTED]
  DataAccess/AppDbContext.cs → Models/Banner.cs
- `AppDbContext` --references--> `BannerProduct`  [EXTRACTED]
  DataAccess/AppDbContext.cs → Models/Banner.cs
- `AppDbContext` --references--> `Cart`  [EXTRACTED]
  DataAccess/AppDbContext.cs → Models/Cart.cs

## Import Cycles
- None detected.

## Communities (49 total, 12 thin omitted)

### Community 0 - "ECommerce2.DTOs"
Cohesion: 0.06
Nodes (27): UploadsController, ECommerce2.Controllers.Customer, ECommerce2.Repositories, ECommerce2.Controllers.Admin, ECommerce2.DataAccess, ECommerce2.Services, ECommerce2.Utilities, E_commerce2 (+19 more)

### Community 1 - "Result"
Cohesion: 0.07
Nodes (31): IDisposable, IProductRepository, List, Task, AttributeService, List, Task, BannerService (+23 more)

### Community 2 - "Coupon"
Cohesion: 0.06
Nodes (34): HttpDelete, HttpGet, HttpPost, HttpPut, IActionResult, Task, CouponsController, UpdateCouponStatusRequest (+26 more)

### Community 3 - "ControllerBase"
Cohesion: 0.07
Nodes (32): ControllerBase, HttpGet, HttpPatch, IActionResult, Task, OrdersController, UpdateNotesRequest, HttpGet (+24 more)

### Community 4 - "IAttributeService"
Cohesion: 0.07
Nodes (31): ActionResult, HttpDelete, HttpGet, HttpPost, HttpPut, IActionResult, List, Task (+23 more)

### Community 5 - "Task"
Cohesion: 0.07
Nodes (28): Authorize, HttpGet, HttpPut, IActionResult, Task, ReviewsController, HttpPost, IActionResult (+20 more)

### Community 6 - "Category"
Cohesion: 0.08
Nodes (25): ActionResult, HttpDelete, HttpGet, HttpPost, HttpPut, IActionResult, Task, CategoriesController (+17 more)

### Community 7 - "IProductService"
Cohesion: 0.09
Nodes (21): HttpDelete, HttpGet, HttpPatch, HttpPost, HttpPut, IActionResult, Task, ProductsController (+13 more)

### Community 8 - "ShippingController"
Cohesion: 0.14
Nodes (14): HttpDelete, HttpGet, HttpPost, HttpPut, IActionResult, Task, ShippingController, CreateGovernorateDto (+6 more)

### Community 9 - "CartController"
Cohesion: 0.14
Nodes (13): ActionResult, HttpDelete, HttpGet, HttpPost, HttpPut, IActionResult, Task, CartController (+5 more)

### Community 10 - "ProfileController"
Cohesion: 0.16
Nodes (13): ActionResult, HttpDelete, HttpGet, HttpPost, HttpPut, IActionResult, Task, ProfileController (+5 more)

### Community 11 - "Task"
Cohesion: 0.21
Nodes (7): OrderDetailsDto, IReadOnlyList, Task, IOrderRepository, IReadOnlyList, Task, OrderService

### Community 12 - "Product"
Cohesion: 0.16
Nodes (13): EntityTypeBuilder, ProductConfiguration, ProductImageConfiguration, ProductVariantConfiguration, BannerProduct, ICollection, Product, ProductImage (+5 more)

### Community 13 - "AuthService"
Cohesion: 0.19
Nodes (12): HttpPost, IActionResult, Task, AuthController, AuthResponseDto, LoginDto, RegisterDto, IConfiguration (+4 more)

### Community 14 - "Task"
Cohesion: 0.17
Nodes (11): ECommerce2.DataAccess.Configurations, EntityTypeBuilder, OrderConfiguration, OrderItemConfiguration, IEntityTypeConfiguration, DateTime, ICollection, Order (+3 more)

### Community 15 - ".GetFavorites"
Cohesion: 0.18
Nodes (10): ActionResult, HttpDelete, HttpGet, HttpPost, IActionResult, List, Task, FavoritesController (+2 more)

### Community 16 - "IRepositories.cs"
Cohesion: 0.17
Nodes (11): IdentityUser, ICollection, Governorate, DateTime, ICollection, User, UserAddress, IGovernorateRepository (+3 more)

### Community 17 - "http"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 18 - "IBannerService"
Cohesion: 0.14
Nodes (14): HttpDelete, HttpPost, HttpPut, IActionResult, Task, BannersController, ActionResult, HttpGet (+6 more)

### Community 19 - "AppDbContext"
Cohesion: 0.18
Nodes (8): CancellationToken, DbSet, ModelBuilder, Task, AppDbContext, IdentityDbContext, Task, UnitOfWork

### Community 20 - ".GetCustomers"
Cohesion: 0.16
Nodes (10): ActionResult, HttpGet, Task, CustomersController, DateTime, AdminCustomerSummaryDto, Task, UserManager (+2 more)

### Community 21 - "PaginatedList"
Cohesion: 0.24
Nodes (6): AdminProductSummaryDto, ProductQueryParameters, IQueryable, List, Task, PaginatedList

### Community 22 - "Color"
Cohesion: 0.27
Nodes (7): ICollection, Color, Size, IColorRepository, ISizeRepository, ColorRepository, SizeRepository

### Community 23 - "GenericRepository"
Cohesion: 0.23
Nodes (6): DbSet, Expression, Func, IReadOnlyList, Task, GenericRepository

### Community 24 - "IGenericRepository"
Cohesion: 0.23
Nodes (5): Expression, Func, IReadOnlyList, Task, IGenericRepository

### Community 25 - "Banner"
Cohesion: 0.29
Nodes (6): BannerQueryParameters, DateTime, ICollection, Banner, IBannerRepository, BannerRepository

### Community 26 - "BaseEntity"
Cohesion: 0.27
Nodes (7): DateTime, BaseEntity, ICollection, Cart, CartItem, ICartRepository, CartRepository

### Community 27 - "E-commerce2.csproj"
Cohesion: 0.20
Nodes (9): net9.0, Microsoft.AspNetCore.Authentication.JwtBearer (9.0.0), Microsoft.AspNetCore.Identity.EntityFrameworkCore (9.0.18), Microsoft.AspNetCore.OpenApi (9.0.9), Microsoft.EntityFrameworkCore (9.0.18), Microsoft.EntityFrameworkCore.SqlServer (9.0.18), Microsoft.EntityFrameworkCore.Tools (9.0.18), Scalar.AspNetCore (2.16.17) (+1 more)

### Community 28 - ".ValidateCoupon"
Cohesion: 0.32
Nodes (5): HttpPost, IActionResult, Task, CouponsController, ICouponService

### Community 29 - "Favorite"
Cohesion: 0.43
Nodes (3): Favorite, IFavoriteRepository, FavoriteRepository

### Community 30 - "Review"
Cohesion: 0.43
Nodes (3): Review, IReviewRepository, ReviewRepository

### Community 31 - ".GetBanners"
Cohesion: 0.40
Nodes (3): ActionResult, HttpGet, BannerDto

### Community 35 - "Migration"
Cohesion: 0.50
Nodes (3): Migration, MigrationBuilder, InitialCreate

### Community 41 - "AppDbContextModelSnapshot.cs"
Cohesion: 0.40
Nodes (3): ModelBuilder, AppDbContextModelSnapshot, ModelSnapshot

### Community 42 - "StoreSetting"
Cohesion: 0.60
Nodes (3): StoreSetting, IStoreSettingRepository, StoreSettingRepository

### Community 43 - "SD.cs"
Cohesion: 0.50
Nodes (3): E_commerce2.Utilities, string, SD

### Community 45 - "E_commerce2.Migrations"
Cohesion: 0.33
Nodes (3): E_commerce2.Migrations, ModelBuilder, AddAdminNotesToOrder

## Knowledge Gaps
- **29 isolated node(s):** `UpdateNotesRequest`, `UpdateStatusRequest`, `CartItemDto`, `CreateOrderItemDto`, `OrderItemDto` (+24 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **12 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Result` connect `Result` to `ECommerce2.DTOs`, `Coupon`, `ControllerBase`, `IAttributeService`, `Task`, `Category`, `IProductService`, `ShippingController`, `CartController`, `ProfileController`, `Task`, `AuthService`, `.GetFavorites`, `IBannerService`, `.ValidateCoupon`?**
  _High betweenness centrality (0.172) - this node is a cross-community bridge._
- **Why does `ECommerce2.DataAccess` connect `ECommerce2.DTOs` to `20260808081228_Phase3_Final_Models.Designer.cs`, `AppDbContextModelSnapshot.cs`, `20260808033532_InitialCreate.Designer.cs`, `E_commerce2.Migrations`, `20260808070306_AddShippingEntities.Designer.cs`, `20260808071419_AddReviewsTable.Designer.cs`, `20260808072512_AddFavoritesTable.Designer.cs`, `20260808073809_AddCreatedAtToUser.Designer.cs`?**
  _High betweenness centrality (0.141) - this node is a cross-community bridge._
- **What connects `UpdateNotesRequest`, `UpdateStatusRequest`, `CartItemDto` to the rest of the system?**
  _29 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ECommerce2.DTOs` be split into smaller, more focused modules?**
  _Cohesion score 0.06406112253893623 - nodes in this community are weakly interconnected._
- **Should `Result` be split into smaller, more focused modules?**
  _Cohesion score 0.07469135802469136 - nodes in this community are weakly interconnected._
- **Should `Coupon` be split into smaller, more focused modules?**
  _Cohesion score 0.05737704918032787 - nodes in this community are weakly interconnected._
- **Should `ControllerBase` be split into smaller, more focused modules?**
  _Cohesion score 0.06636500754147813 - nodes in this community are weakly interconnected._