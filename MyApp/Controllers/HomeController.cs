using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.ViewModels;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeRepository _empRepository;

        public HomeController(IEmployeRepository employeRepository)
        {
            _empRepository = employeRepository;
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
    }
}
