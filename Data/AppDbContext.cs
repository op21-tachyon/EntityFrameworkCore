using EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Book>().HasData(
            //    new Book { Id = 1, Title = "Harry Potter 1", Description = "Description for Harry Potter 1", isActive = "true", CreatedOn = DateTime.Now},
            //    new Book { Id = 2, Title = "Harry Potter 2", Description = "Description for Harry Potter 2", isActive = "true", CreatedOn = DateTime.Now },
            //    new Book { Id = 3, Title = "Harry Potter 3", Description = "Description for Harry Potter 3", isActive = "true", CreatedOn = DateTime.Now });

            //modelBuilder.Entity<Currency>().HasData(
            //    new Currency { Id = 1, Title = "USD", Description="US Dollar", bookPrices = null },
            //    new Currency { Id = 2, Title = "EUR", Description = "Euro", bookPrices = null },
            //    new Currency { Id = 3, Title = "Rupee", Description = "Indian Rupee", bookPrices = null });

            //modelBuilder.Entity<Language>().HasData(
            //    new Language { Id = 1, Title = "English", Description = "English Language" },
            //    new Language { Id = 2, Title = "French", Description = "French Language" },
            //    new Language { Id = 3, Title = "Hindi", Description = "Hindi Language" });
        }
        // Define your DbSets here
        public DbSet<Book> Books { get; set; }    
        public DbSet<Currency> Currencies { get; set; }    
        public DbSet<Language> Languages { get; set; }    
        public DbSet<Author> Authors { get; set; }    
    }
}
