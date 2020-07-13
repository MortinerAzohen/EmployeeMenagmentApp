using Microsoft.AspNetCore.Http;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format")]
        [Display(Name = "Office E-mail")]
        public string Email { get; set; }
        [Required]
        public Dpt? Departmennt { get; set; }
        public string FullName
        {
            get { return Surname + ", " + Name; }
        }
        public List<IFormFile> Photos { get; set; }
        public Depo Depo { get; set; }

    }
}
