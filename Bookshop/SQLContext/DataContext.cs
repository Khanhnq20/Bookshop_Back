using Bookshop.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookshop.Entity;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace Bookshop.SQLContext
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Format> Formats { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<ProductGenre> ProductGenres { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public virtual DbSet<PurchasedProduct> PurchasedProducts { get; set; }
    }
}
