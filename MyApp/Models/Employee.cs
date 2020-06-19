using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage="Invalid email format")]
        [Display(Name = "Office E-mail")]
        public string Email { get; set; }
        [Required]
        public Dpt? Departmennt { get; set; }
        public string FullName {
            get { return Surname+", "+Name; }
        }
    }
}
