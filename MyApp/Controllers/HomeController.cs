using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.ViewModels;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeRepository _empRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeRepository employeRepository, IHostingEnvironment hostingEnvironment)
        {
            _empRepository = employeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _empRepository.employeesGetAll();
            return View(model);
        }

        public ViewResult Details(int id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _empRepository.GetEmployee(id),
                PageTitle = "Employe Details"
            };
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photos != null && model.Photos.Count>0)
                {
                    foreach(IFormFile photo in model.Photos)
                    {

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    photo.CopyTo(new FileStream(filePath,FileMode.Create));
                    }
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Surname = model.Surname,
                    Departmennt = model.Departmennt,
                    PhotoPath = uniqueFileName
                };
                _empRepository.Add(newEmployee);

                return RedirectToAction("details", new { id = newEmployee.ID });
            }
            return View();
        }
    }
}
