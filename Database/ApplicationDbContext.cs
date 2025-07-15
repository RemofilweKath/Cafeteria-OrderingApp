using CafeteriaOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee unique constraint
            modelBuilder.Entity<Employee>()
                .HasIndex(employee => employee.EmployeeNumber)
                .IsUnique();
        }
    }
}
