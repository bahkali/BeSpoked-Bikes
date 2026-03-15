using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class BeSpokedContext : DbContext
{
    public BeSpokedContext(DbContextOptions<BeSpokedContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Salesperson> Salespersons => Set<Salesperson>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<Discount> Discounts => Set<Discount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique indexes
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Name, p.Manufacturer })
            .IsUnique();

        modelBuilder.Entity<Salesperson>()
            .HasIndex(s => new { s.FirstName, s.LastName, s.Phone })
            .IsUnique();

        // Sale FK relationships — Restrict delete
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Salesperson)
            .WithMany(sp => sp.Sales)
            .HasForeignKey(s => s.SalespersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Discount FK — Cascade delete
        modelBuilder.Entity<Discount>()
            .HasOne(d => d.Product)
            .WithMany(p => p.Discounts)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // ── Products ──
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Trail Blazer 3000", Manufacturer = "Trek", Style = "Mountain", PurchasePrice = 1200.00m, SalePrice = 1899.99m, QtyOnHand = 10, CommissionPercentage = 10.00m },
            new Product { Id = 2, Name = "Road Master Elite", Manufacturer = "Specialized", Style = "Road", PurchasePrice = 2000.00m, SalePrice = 3199.99m, QtyOnHand = 7, CommissionPercentage = 12.00m },
            new Product { Id = 3, Name = "Urban Glide 500", Manufacturer = "Cannondale", Style = "Hybrid", PurchasePrice = 600.00m, SalePrice = 999.99m, QtyOnHand = 15, CommissionPercentage = 8.00m },
            new Product { Id = 4, Name = "Aero Speed Pro", Manufacturer = "Giant", Style = "Road", PurchasePrice = 3500.00m, SalePrice = 5499.99m, QtyOnHand = 4, CommissionPercentage = 15.00m },
            new Product { Id = 5, Name = "Fat Tire Explorer", Manufacturer = "Surly", Style = "Fat Bike", PurchasePrice = 900.00m, SalePrice = 1499.99m, QtyOnHand = 8, CommissionPercentage = 9.00m }
        );

        // ── Salespersons ──
        modelBuilder.Entity<Salesperson>().HasData(
            new Salesperson { Id = 1, FirstName = "James", LastName = "Carter", Address = "123 Peachtree St, Atlanta, GA 30301", Phone = "404-555-0101", StartDate = new DateTime(2022, 3, 15), Manager = "Sarah Mitchell" },
            new Salesperson { Id = 2, FirstName = "Olivia", LastName = "Chen", Address = "456 Buckhead Ave, Atlanta, GA 30305", Phone = "404-555-0202", StartDate = new DateTime(2021, 6, 1), Manager = "Sarah Mitchell" },
            new Salesperson { Id = 3, FirstName = "Marcus", LastName = "Johnson", Address = "789 Midtown Blvd, Atlanta, GA 30308", Phone = "404-555-0303", StartDate = new DateTime(2023, 1, 10), Manager = "Sarah Mitchell" },
            new Salesperson { Id = 4, FirstName = "Elena", LastName = "Rodriguez", Address = "321 Decatur Rd, Atlanta, GA 30307", Phone = "404-555-0404", StartDate = new DateTime(2020, 9, 20), TerminationDate = new DateTime(2024, 12, 31), Manager = "Sarah Mitchell" }
        );

        // ── Customers ──
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "David", LastName = "Thompson", Address = "100 Piedmont Park Dr, Atlanta, GA 30309", Phone = "678-555-1001", StartDate = new DateTime(2023, 2, 10) },
            new Customer { Id = 2, FirstName = "Sarah", LastName = "Williams", Address = "250 Virginia Ave, Atlanta, GA 30306", Phone = "678-555-1002", StartDate = new DateTime(2023, 5, 22) },
            new Customer { Id = 3, FirstName = "Michael", LastName = "Brown", Address = "88 Ponce De Leon Ave, Atlanta, GA 30308", Phone = "770-555-1003", StartDate = new DateTime(2024, 1, 5) },
            new Customer { Id = 4, FirstName = "Jessica", LastName = "Davis", Address = "415 Grant Park Way, Atlanta, GA 30312", Phone = "770-555-1004", StartDate = new DateTime(2024, 3, 18) },
            new Customer { Id = 5, FirstName = "Robert", LastName = "Garcia", Address = "60 Inman Park Ln, Atlanta, GA 30307", Phone = "404-555-1005", StartDate = new DateTime(2024, 8, 1) }
        );

        // ── Discounts (active Q1 2025) ──
        modelBuilder.Entity<Discount>().HasData(
            new Discount { Id = 1, ProductId = 1, BeginDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 3, 31), DiscountPercentage = 10.00m },
            new Discount { Id = 2, ProductId = 3, BeginDate = new DateTime(2025, 2, 1), EndDate = new DateTime(2025, 4, 30), DiscountPercentage = 15.00m },
            new Discount { Id = 3, ProductId = 5, BeginDate = new DateTime(2025, 3, 1), EndDate = new DateTime(2025, 3, 31), DiscountPercentage = 5.00m }
        );

        // ── Sales ──
        // Pre-calculated Price (after discount) and Commission
        modelBuilder.Entity<Sale>().HasData(
            // Jan 12 — Trail Blazer (10% off): 1899.99 × 0.90 = 1709.99, commission 10% = 171.00
            new Sale { Id = 1, ProductId = 1, SalespersonId = 1, CustomerId = 1, SalesDate = new DateTime(2025, 1, 12), Price = 1709.99m, SalespersonCommission = 171.00m },
            // Jan 25 — Road Master (no discount): 3199.99, commission 12% = 384.00
            new Sale { Id = 2, ProductId = 2, SalespersonId = 2, CustomerId = 2, SalesDate = new DateTime(2025, 1, 25), Price = 3199.99m, SalespersonCommission = 384.00m },
            // Feb 10 — Urban Glide (15% off): 999.99 × 0.85 = 849.99, commission 8% = 68.00
            new Sale { Id = 3, ProductId = 3, SalespersonId = 1, CustomerId = 3, SalesDate = new DateTime(2025, 2, 10), Price = 849.99m, SalespersonCommission = 68.00m },
            // Feb 20 — Aero Speed (no discount): 5499.99, commission 15% = 825.00
            new Sale { Id = 4, ProductId = 4, SalespersonId = 3, CustomerId = 4, SalesDate = new DateTime(2025, 2, 20), Price = 5499.99m, SalespersonCommission = 825.00m },
            // Mar 5 — Trail Blazer (10% off): 1899.99 × 0.90 = 1709.99, commission 10% = 171.00
            new Sale { Id = 5, ProductId = 1, SalespersonId = 2, CustomerId = 5, SalesDate = new DateTime(2025, 3, 5), Price = 1709.99m, SalespersonCommission = 171.00m },
            // Mar 15 — Fat Tire (5% off): 1499.99 × 0.95 = 1424.99, commission 9% = 128.25
            new Sale { Id = 6, ProductId = 5, SalespersonId = 3, CustomerId = 1, SalesDate = new DateTime(2025, 3, 15), Price = 1424.99m, SalespersonCommission = 128.25m }
        );
    }
}
