using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Migrations;
using MyApp.Models;
using MyApp.Models.Department;
using MyApp.ViewModels;

namespace MyApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeRepository _empRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IDepartmentRepository _departmentRepository;

        public HomeController(IEmployeRepository employeRepository, IHostingEnvironment hostingEnvironment, IDepartmentRepository departmentRepository)
        {
            _empRepository = employeRepository;
            this.hostingEnvironment = hostingEnvironment;
            _departmentRepository = departmentRepository;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _empRepository.employeesGetAll();
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Index(string searchString = null)
        {
            ViewData["GetEmployees"] = searchString;
            var empQuery = _empRepository.employeesGetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                empQuery = _departmentRepository.FindEmployees(searchString);
            }
            return View(empQuery);
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            //throw new Exception("this is error msg");
            Employee emp = _empRepository.GetEmployee(id.Value);
            if (emp == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = emp,
                PageTitle = "Employe Details"
            };
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _empRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.ID,
                Name = employee.Name,
                Surname = employee.Surname,
                Email = employee.Email,
                Departmennt = employee.Departmennt,
                existingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = (int)model.Departmennt;
                Employee employee = _empRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Surname = model.Surname;
                employee.Email = model.Email;
                employee.Departmennt = model.Departmennt;
                employee.Depo = _departmentRepository.FindDepartment(id);

                if (model.Photos != null && model.Photos.Count > 0)
                {
                    if (model.existingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.existingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                     employee.PhotoPath = ProcessUploadedFile(model);
                }
                _empRepository.Upadate(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                int id = (int)model.Departmennt;
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Surname = model.Surname,
                    Departmennt = model.Departmennt,
                    PhotoPath = uniqueFileName,
                    Depo = _departmentRepository.FindDepartment(id)
                };
                _empRepository.Add(newEmployee);

                return RedirectToAction("details", new { id = newEmployee.ID });
            }
            return View();
        }
        public IActionResult Delete(int Id)
        {
            _empRepository.Delete(Id);
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public IActionResult Departments()
        {
            var model = _departmentRepository.GetAllDepartments();
            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult DepartmentView(int id)
        {
            //throw new Exception("this is error msg");
            Depo depo = _departmentRepository.GetDepartment(id);
            DepartmentViewModel model = new DepartmentViewModel
            {
                Depo = depo
            };
            return View(model);
        }

    }
}
