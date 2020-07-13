using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Models.Department
{
    public interface IDepartmentRepository
    {
        Depo GetDepartment(int Id);
        IEnumerable<Depo> GetAllDepartments();
        IEnumerable<Employee> FindEmployees(string name);
        Depo FindDepartment(int id);
    }
}
