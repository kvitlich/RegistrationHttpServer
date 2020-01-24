using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistrationServer
{
    class RegistrationDbContext : DbContext
    {
        public RegistrationDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=KVITLICH;Database=UsersRegistration;Trusted_Connection=true;");
        }
    }
}
