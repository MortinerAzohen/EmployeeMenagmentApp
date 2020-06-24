using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AccountController:Controller
    {
        private readonly UserManager<ApplicationUser> userMenager;
        private readonly SignInManager<ApplicationUser> signInMenager;

        public AccountController(UserManager<ApplicationUser> userMenager, SignInManager<ApplicationUser> signInMenager)
        {
            this.userMenager = userMenager;
            this.signInMenager = signInMenager;
           
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, City = model.City};
                var result = await userMenager.CreateAsync(user,model.Password);
                if(result.Succeeded)
                {
                    await signInMenager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInMenager.SignOutAsync();
            return RedirectToAction("index","home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await signInMenager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(model);
        }
        [HttpPost][HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userMenager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Emaill {email} is already in use");
            }
        }
    }
}
