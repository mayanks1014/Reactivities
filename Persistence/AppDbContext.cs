using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Value> Values { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>().HasData(new Value { Id = 1, Name = "Value101"}, new Value { Id= 2, Name= "Value102"});

            base.OnModelCreating(modelBuilder);
        }
    }
}
