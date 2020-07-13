using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class Depo
    {
        public Depo()
        {
            Employees = new List<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
