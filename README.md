# Entity Framework Core Learning Project

A comprehensive ASP.NET Core Web API project built to learn and practice **Entity Framework Core (EF Core)** concepts. This project demonstrates various EF Core patterns, query techniques, and database operations with practical examples.

---

## 📋 Table of Contents

1. [Project Overview](#project-overview)
2. [Architecture & Structure](#architecture--structure)
3. [Database Models](#database-models)
4. [Core Concepts Implemented](#core-concepts-implemented)
5. [API Endpoints Documentation](#api-endpoints-documentation)
6. [Code Examples & Patterns](#code-examples--patterns)
7. [Setup & Running](#setup--running)

---

## 🎯 Project Overview

**Technology Stack:**
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (ORM)
- SQL Server
- Swagger/OpenAPI for API documentation

**Purpose:** Learning practical EF Core patterns and techniques for consuming and manipulating data through a REST API.

---

## 🏗️ Architecture & Structure

### Project Layout
```
EntityFrameworkCore/
├── Models/                  # Domain models
│   ├── Book.cs
│   ├── Author.cs
│   ├── Currency.cs
│   └── Language.cs
├── Data/
│   └── AppDbContext.cs      # EF Core DbContext configuration
├── Controllers/             # API endpoints
│   ├── BookController.cs
│   ├── CurrencyController.cs
│   └── LanguageController.cs
├── Migrations/              # EF Core database migrations
├── Program.cs               # Dependency injection & middleware setup
└── appsettings.json         # Configuration
```

### Database Schema

**Tables:**
1. **Books** - Main book entity with title, description, and relationships
2. **Authors** - Author information linked to books (One-to-Many relationship)
3. **Currencies** - Currency types for book pricing
4. **Languages** - Supported languages for books

---

## 📦 Database Models

### 1. **Book Model**
```csharp
public class Book
{
	[Required]
	public int Id { get; set; }                      // Primary Key
	public string Title { get; set; }                 // Book title
	public string Description { get; set; }           // Book description
	public string isActive { get; set; }              // Status (active/inactive)
	public DateTime CreatedOn { get; set; }           // Creation timestamp

	public int? AuthorId { get; set; }                // Foreign Key (nullable)
	public virtual Author? Author { get; set; }       // Navigation property
}
```
**Concepts:** Required attributes, navigation properties, foreign keys, one-to-many relationships

### 2. **Author Model**
```csharp
public class Author
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
}
```

### 3. **Currency Model**
```csharp
public class Currency
{
	[Required]
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public int? bookPrices { get; set; }
}
```

### 4. **Language Model**
```csharp
public class Language
{
	[Required]
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}
```

---

## 🚀 Core Concepts Implemented

### 1. **DbContext Configuration**
**File:** `Data/AppDbContext.cs`

```csharp
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Book> Books { get; set; }
	public DbSet<Currency> Currencies { get; set; }
	public DbSet<Language> Languages { get; set; }
	public DbSet<Author> Authors { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Data seeding (commented out examples provided)
		// Entity configurations
	}
}
```

**Concepts:**
- DbContext inheritance and initialization
- DbSet<T> collections for each entity
- OnModelCreating override for configurations
- Dependency injection through constructor

---

### 2. **Data Loading Patterns** (Relationship Loading)

#### ✅ **Eager Loading**
Fetch related entities in a single query using `.Include()`

```csharp
// Book with Author - single query
var result = await _appDbContext.Books
	.Include(x => x.Author)
	.ToListAsync();
```

**Use Case:** When you know you'll need related data and want optimal performance

---

#### ✅ **Explicit Loading**
Explicitly load related entities after retrieving the main entity

```csharp
// Get book first
var result = await _appDbContext.Books.FirstAsync();

// Load author separately
await _appDbContext.Entry(result).Reference(x => x.Author).LoadAsync();
```

**Use Case:** When you conditionally need related data after fetching the main entity

---

#### ✅ **Lazy Loading**
Related entities are loaded automatically when accessed (requires proxies)

```csharp
// Program.cs configuration
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseLazyLoadingProxies()
		   .UseSqlServer(connectionString));

// Usage - Author loads automatically when accessed
var books = await _appDbContext.Books.ToListAsync();
foreach(var book in books)
{
	var author = book.Author; // Loads automatically
}
```

**⚠️ Warning:** Can cause performance issues (N+1 queries) if not used carefully

---

### 3. **Query Patterns**

#### ✅ **LINQ to Entities (Method Syntax)**
```csharp
var result = _appDbContext.Books
	.Where(x => x.Id > 5)
	.Select(x => new { x.Title, x.Author })
	.AsNoTracking()
	.ToList();
```

#### ✅ **LINQ Query Syntax**
```csharp
var result = (from books in _appDbContext.Books
			  select books).ToList();
```

#### ✅ **Raw SQL Queries**
```csharp
// Basic SQL query
var result = _appDbContext.Books
	.FromSql($"SELECT * FROM Books")
	.ToList();
```

#### ✅ **Stored Procedure Execution**
```csharp
// Get all books from stored procedure
var result = _appDbContext.Books
	.FromSql($"EXEC SP_GetBooks")
	.ToList();

// Get book by ID with parameter
var param = new SqlParameter("@Id", 1);
var result = _appDbContext.Books
	.FromSql($"EXEC SP_GetBookById {param}")
	.ToList();
```

---

### 4. **Filtering & Searching Patterns**

#### ✅ **Search by Multiple Conditions**
```csharp
var result = await _appDbContext.Currencies
	.Where(u => u.Title == name && 
		   (string.IsNullOrEmpty(description) || u.Description == description))
	.ToListAsync();
```

#### ✅ **Search by Collection (Contains)**
```csharp
List<int> ids = new List<int> { 1, 2, 3 };
var result = await _appDbContext.Currencies
	.Where(u => ids.Contains(u.Id))
	.ToListAsync();
```

---

### 5. **Query Optimization**

#### ✅ **AsNoTracking() - For Read-Only Queries**
```csharp
// Better performance for queries that don't need change tracking
var result = await _appDbContext.Currencies
	.AsNoTracking()
	.ToListAsync();
```

**Benefit:** Reduces memory overhead when you're not modifying entities

#### ✅ **Projection - Select Specific Fields**
```csharp
// Only fetch needed columns instead of entire entity
var result = await _appDbContext.Currencies
	.Where(u => ids.Contains(u.Id))
	.Select(u => new { u.Title, u.Description })
	.ToListAsync();
```

---

### 6. **Query Result Retrieval Methods**

| Method | Purpose | Returns | Notes |
|--------|---------|---------|-------|
| `FirstOrDefault()` | Get first record or null | Single item or null | Good for optional retrieval |
| `SingleOrDefault()` | Get one unique record or null | Single item or null | Throws if multiple matches |
| `FindAsync(id)` | Get by primary key | Single item or null | Most efficient for PK lookup |
| `ToList()` / `ToListAsync()` | Get all matching records | List<T> | Loads all into memory |
| `Single()` | Get one unique record | Single item | Throws if not found/multiple |

---

### 7. **Create Operations (INSERT)**

#### ✅ **Add Single Entity**
```csharp
var book = new Book { Title = "...", CreatedOn = DateTime.Now };
_appDbContext.Books.Add(book);
await _appDbContext.SaveChangesAsync(); // Single database call
```

#### ✅ **Add Multiple Entities (Bulk Insert)**
```csharp
var books = new List<Book> { ... };
_appDbContext.Books.AddRange(books);
await _appDbContext.SaveChangesAsync(); // Single batch call
```

**Concepts:**
- DbSet<T>.Add() for single entity
- DbSet<T>.AddRange() for bulk operations
- SaveChangesAsync() persists to database

---

### 8. **Update Operations (UPDATE)**

#### ✅ **Traditional Update (Track & Save)**
```csharp
var book = _appDbContext.Books.FirstOrDefault(x => x.Id == id);
if (book != null)
{
	book.Title = newTitle;
	book.Description = newDescription;
	await _appDbContext.SaveChangesAsync();
}
```

**Concepts:** Change tracking for automatic state management

---

#### ✅ **Direct Update (Single Query)**
```csharp
_appDbContext.Books.Update(book);
await _appDbContext.SaveChangesAsync();
```

---

#### ✅ **Bulk Update (ExecuteUpdateAsync)**
```csharp
var result = await _appDbContext.Books
	.Where(b => b.Title == "Harry Potter 3")
	.ExecuteUpdateAsync(b => b
		.SetProperty(p => p.Description, 
					 p => p.Title + " " + "Updated Description")
	);
```

**Benefit:** Single database call for bulk updates without loading entities into memory

---

### 9. **Delete Operations (DELETE)**

#### ✅ **Soft Delete (Logical Delete)**
```csharp
var result = await _appDbContext.Books
	.Where(x => x.Id == id)
	.ExecuteUpdateAsync(x => x.SetProperty(b => b.isActive, b => "0"));
```

**Use Case:** Keep record in database but mark as inactive for audit purposes

---

#### ✅ **Hard Delete - Method 1 (Fetch & Remove)**
```csharp
var book = await _appDbContext.Books.FindAsync(id);
if (book != null)
{
	_appDbContext.Books.Remove(book);
	await _appDbContext.SaveChangesAsync();
}
```

---

#### ✅ **Hard Delete - Method 2 (State-Based Delete - Single Query)**
```csharp
// Most efficient - no need to fetch record first
var book = new Book { Id = id };
_appDbContext.Entry(book).State = EntityState.Deleted;
await _appDbContext.SaveChangesAsync();
```

**Benefit:** Only one database call, no unnecessary SELECT

---

#### ✅ **Bulk Delete (ExecuteDeleteAsync)**
```csharp
var result = await _appDbContext.Books
	.Where(x => x.Id == 5)
	.ExecuteDeleteAsync();
```

**Benefit:** Single query to delete multiple records

---

### 10. **Database Command Execution**

#### ✅ **Execute Custom SQL Updates**
```csharp
var result = await _appDbContext.Database
	.ExecuteSqlAsync($"Update Books set AuthorId = 2 where Id = 5");
```

**Use Case:** Complex updates that are difficult to express with LINQ

---

### 11. **Entity State Management**

| State | Meaning | When Applied |
|-------|---------|--------------|
| `Added` | New entity | When `.Add()` or `.AddRange()` called |
| `Modified` | Entity properties changed | Change tracker detects changes |
| `Deleted` | Entity marked for deletion | When `.Remove()` called or State set |
| `Detached` | Not tracked by context | Never added or removed from tracking |
| `Unchanged` | Loaded but not modified | Initial state after query |

---

### 12. **DbContext Configuration (Program.cs)**

```csharp
// Standard SQL Server connection
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// With Lazy Loading Proxies enabled
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseLazyLoadingProxies()
		   .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

---

### 13. **Data Seeding** (OnModelCreating)

```csharp
// Seed initial data during migration
modelBuilder.Entity<Book>().HasData(
	new Book 
	{ 
		Id = 1, 
		Title = "Harry Potter 1", 
		Description = "...", 
		isActive = "true", 
		CreatedOn = DateTime.Now
	}
);
```

---

### 14. **Database Migrations**

Migration files in `Migrations/` folder track schema changes:

- `20260830063034_BooksTable.cs` - Initial Books table
- `20260830063615_DataSeedBooksTable.cs` - Seed book data
- `20260830064725_DataSeedCurrencyTable.cs` - Currency data
- `20260830070711_LanguageTable.cs` - Language table
- `20260830073031_AddingPKinAllTables.cs` - Added primary keys
- `20260830132024_BookCreateOnDateTime.cs` - Added CreatedOn field
- `20260830170156_NewAuthTable.cs` - Added Author table

**Common Commands:**
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

---

## 📡 API Endpoints Documentation

### **BookController** (`/api/book`)

| Method | Endpoint | Description | Example |
|--------|----------|-------------|---------|
| GET | `/allasync` | Get all books asynchronously with author details | Returns books with author info |
| GET | `/all` | Get all books synchronously | Returns all books |
| POST | `/Addbook` | Add a single book | `{ "title": "...", "description": "..." }` |
| POST | `/books` | Add multiple books in bulk | `[{ ... }, { ... }]` |
| PUT | `/{id}` | Update book by ID | `{ "title": "...", ... }` |
| PUT | `/UpdatebookWithSingleQuery` | Update book efficiently | `{ "id": 1, "title": "..." }` |
| PUT | `/UpdatebookInBulk` | Bulk update books | Updates all matching criteria |
| DELETE | `/{id}` | Delete book by ID (hard delete) | Returns deleted book |
| DELETE | `/bulk` | Delete multiple books | Deletes all matching criteria |

---

### **CurrencyController** (`/api/currency`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/GetAllCurrencies` | Get all currencies synchronously |
| GET | `/GetAllCurrenciesAynch` | Get all currencies asynchronously |
| GET | `/{Id:int}` | Get currency by ID |
| GET | `/{name}` | Get currencies by name (returns list) |
| GET | `/{name}/{description}` | Get currencies by name and description |
| POST | `/all` | Get currencies by list of IDs with projection |

---

### **LanguageController** (`/api/language`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all languages |

---

## 💡 Code Examples & Patterns

### Pattern 1: Async/Await Pattern (Best Practice)
```csharp
[HttpGet("allasync")]
public async Task<IActionResult> GetBooksAsync()
{
	var result = await _appDbContext.Books
		.AsNoTracking()
		.ToListAsync();
	return Ok(result);
}
```

---

### Pattern 2: Dependency Injection
```csharp
public class BookController : ControllerBase
{
	private readonly AppDbContext _appDbContext;

	public BookController(AppDbContext appContext)
	{
		_appDbContext = appContext;
	}
}
```

---

### Pattern 3: Safe Null Checking
```csharp
var objBook = _appDbContext.Books.FirstOrDefault(x => x.Id == id);
if (objBook == null)
{
	return NotFound();
}
// Process book
```

---

### Pattern 4: Projection for Performance
```csharp
// ❌ Loads entire entity
var all = await _appDbContext.Currencies.ToListAsync();

// ✅ Loads only needed fields
var optimized = await _appDbContext.Currencies
	.Select(u => new { u.Title, u.Description })
	.ToListAsync();
```

---

### Pattern 5: Conditional Filtering
```csharp
var result = await _appDbContext.Currencies
	.Where(u => u.Title == name && 
		   (string.IsNullOrEmpty(description) || u.Description == description))
	.ToListAsync();
```

---

## 🔧 Setup & Running

### Prerequisites
- .NET 8 SDK
- SQL Server (local or connection string)
- Visual Studio Community 2026

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/op21-tachyon/EntityFrameworkCore.git
   cd EntityFrameworkCore
   ```

2. **Update Connection String**
   - Edit `appsettings.json`:
   ```json
   {
	 "ConnectionStrings": {
	   "DefaultConnection": "Server=YOUR_SERVER;Database=EntityFrameworkCoreDB;Trusted_Connection=true;"
	 }
   }
   ```

3. **Apply Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**
   - Navigate to: `https://localhost:5001/swagger`

---

## 📚 Key Learnings Summary

### ✅ **What You've Learned:**

1. **Entity Framework Core Fundamentals**
   - DbContext configuration and usage
   - Entity relationships (One-to-Many)
   - Navigation properties

2. **Query Techniques**
   - LINQ to Entities (method and query syntax)
   - Raw SQL queries and stored procedures
   - Async/await patterns

3. **Performance Optimization**
   - AsNoTracking() for read-only queries
   - Projection (Select) to limit data
   - Bulk operations (ExecuteUpdateAsync, ExecuteDeleteAsync)
   - Eager loading vs Lazy loading vs Explicit loading

4. **CRUD Operations**
   - CREATE: Add, AddRange
   - READ: FirstOrDefault, SingleOrDefault, Find, ToList
   - UPDATE: Traditional update, direct update, bulk update
   - DELETE: Hard delete, soft delete, bulk delete

5. **Advanced Features**
   - Database migrations and versioning
   - Data seeding
   - Entity state management
   - Change tracking

---

## 🚨 Important Notes & Best Practices

### Performance Considerations

| ❌ Anti-Pattern | ✅ Best Practice |
|------------------|------------------|
| Lazy loading (risk of N+1) | Use Include() or AsNoTracking() |
| Selecting entire entities | Project with Select() |
| Multiple database calls | Use batch operations |
| No connection string | Use configuration management |

### Code Quality Tips

1. **Always use async/await** for database operations
2. **Use AsNoTracking()** for read-only queries
3. **Project with Select()** when you don't need full entities
4. **Use FindAsync()** for primary key lookups
5. **Validate before operations** to avoid unnecessary DB calls
6. **Handle exceptions** gracefully in production

---

## 📖 Further Reading

- [Microsoft EF Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [EF Core Performance Best Practices](https://learn.microsoft.com/en-us/ef/core/performance/)
- [LINQ to Entities Guide](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)

---

## 📝 Project Statistics

- **Total Controllers:** 3
- **Total Models:** 4
- **Total Database Tables:** 4
- **Total Migrations:** 9
- **API Endpoints:** 15+

---

**Last Updated:** August 30, 2026  
**Learning Focus:** Entity Framework Core 8 with ASP.NET Core Web API
