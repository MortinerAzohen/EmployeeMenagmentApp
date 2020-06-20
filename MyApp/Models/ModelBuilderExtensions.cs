using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
    new Employee
    {
        ID = 1,
        Name = "Mark2",
        Departmennt = Dpt.Hr,
        Surname = "Klark",
        Email = "Mark2@gmail.com"
    },
    new Employee
    {
        ID = 2,
        Name = "John",
        Departmennt = Dpt.Hr,
        Surname = "Klark",
        Email = "John@gmail.com"
    }
    );
        }
    }
}
