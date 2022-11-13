using HoTrungNguyenBTTH2.Models;
using Microsoft.EntityFrameworkCore;

namespace HoTrungNguyenBTTH2.Data{
    
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Customer> Customers {get;set;}
        public DbSet<Student> Students {get;set;}
        public DbSet<Person> Persons {get;set;}
        public DbSet<Employee> Employee {get;set;}
    }
}