using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Models.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class SQLDepartmentRepository:IDepartmentRepository
    {
        private AppDbContext context;

        public SQLDepartmentRepository(AppDbContext context)
        {
            this.context = context;
        }


        public IEnumerable<Employee> FindEmployees(string userInstertedValue)
        {
            var input = userInstertedValue.ToLower();

            var employees = context.Employees.Where(e => e.Email.ToLower() == input ||
                                         e.Name.ToLower() == input ||
                                         e.Surname.ToLower() == input);
            return employees;
        }

        public IEnumerable<Depo> GetAllDepartments()
        {
            return context.Depos;
        }

        public Depo GetDepartment(int id)
        {
            var depo = context.Depos
                .Include(d => d.Employees)
                .Single(d => d.Id == id);
            return depo;
        }
        public Depo FindDepartment(int id)
        {
            var depo = context.Depos
                .Single(d => d.Id == id);
            return depo;
                
        }
    }
}
