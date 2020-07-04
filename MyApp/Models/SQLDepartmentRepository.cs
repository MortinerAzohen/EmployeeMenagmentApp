using Microsoft.AspNetCore.Mvc;
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

        public Depo AddEmployeeToDepartment(int Id, Employee emp)
        {
            var depo = context.Depos.Find(Id);
            depo.Employees.Add(emp);
            context.Depos.Update(depo);
            context.SaveChanges();
            return depo;
        }

        public List<Employee> FindEmployeeByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Depo> GetAllDepartments()
        {
            return context.Depos;
        }

        public Depo GetDepartment(int Id)
        {
            return context.Depos.Find(Id);
        }

        public Depo RemoveEmployeeFromDepartment(int Id, int empId)
        {
            var depo = context.Depos.Find(Id);
            var emp = context.Employees.Find(empId);
            depo.Employees.Remove(emp);
            context.Depos.Update(depo);
            context.SaveChanges();
            return depo;
        }
    }
}
