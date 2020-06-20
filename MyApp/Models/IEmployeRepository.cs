using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public interface IEmployeRepository
    {
        Employee GetEmployee(int Id);
        IEnumerable<Employee> employeesGetAll();
        Employee Add(Employee employee);
        Employee Upadate(Employee employeeChanges);
        Employee Delete(int id);
    }
}
