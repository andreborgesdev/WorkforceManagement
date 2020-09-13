using System;
using Microsoft.EntityFrameworkCore;
using WorkforceManagement.Entities;

namespace WorkforceManagement.DataAccess
{
    public class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().HasData(
                new Material()
                {
                    Id = Guid.Parse("40ff5488-fdab-45b5-bc3a-14302d59869a"),
                    Reference = "Hammer",
                    PersonId = Guid.Parse("40ff5488-fdab-45b5-bc3a-14302d59869b")
                }
                );

            modelBuilder.Entity<Person>().HasData(
                new Person()
                {
                    Id = Guid.Parse("40ff5488-fdab-45b5-bc3a-14302d59869b"),
                    FirstName = "André",
                    LastName = "Borges",
                    Address = "Tomorrowland",
                    Contact = "9123132",
                    DateOfBirth = new DateTimeOffset(),
                    Email = "asd@ad.com",
                    NIF = "adasd",
                    Gender = "Male"
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
