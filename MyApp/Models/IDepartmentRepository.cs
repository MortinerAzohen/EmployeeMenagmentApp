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
        Depo AddEmployeeToDepartment( int Id, Employee emp);
        Depo RemoveEmployeeFromDepartment(int Id, int empId);
        List<Employee> FindEmployeeByName(string name);
    }
}
