using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class MockEmpReposiotry : IEmployeRepository
    {
        private List<Employee> _employeeList;
        public MockEmpReposiotry()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() {ID=1,Name="Mary",Surname="Annand",Departmennt ="HR", Email="marya@gmail.com"},
                new Employee() {ID=2,Name="Andyy",Surname="Grand",Departmennt ="IT", Email="marya@gmail.com"},
                new Employee() {ID=3,Name="Kris",Surname="ALexander",Departmennt ="HR", Email="marya@gmail.com"}
            };
        }

        public IEnumerable<Employee> employeesGetAll()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            Employee emp = _employeeList.FirstOrDefault(e => e.ID == Id);
            return emp;
        }
    }
}
