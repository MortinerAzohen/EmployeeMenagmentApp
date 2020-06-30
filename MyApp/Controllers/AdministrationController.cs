using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.ViewModels;

namespace MyApp.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleMenager;
        private readonly UserManager<ApplicationUser> userMenager;

        public AdministrationController(RoleManager<IdentityRole> roleMenager, UserManager<ApplicationUser> userMenager)
        {
            this.roleMenager = roleMenager;
            this.userMenager = userMenager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await roleMenager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleMenager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleMenager.FindByIdAsync(id);
            var model = new EditRoleModelView
            {
                Id = role.Id,
                RoleName = role.Name

            };
            foreach (var user in await userMenager.Users.ToListAsync())
            {
                if (await userMenager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleModelView model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleMenager.FindByIdAsync(model.Id);
                role.Name = model.RoleName;
                var result = await roleMenager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddorRemoveRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleMenager.FindByIdAsync(roleId);

            var model = new List<UserRoleViewModel>();
            foreach (var user in userMenager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    id = user.Id,
                    Name = user.UserName,
                };
                if (await userMenager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddorRemoveRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleMenager.FindByIdAsync(roleId);
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userMenager.FindByIdAsync(model[i].id);

                if (model[i].IsSelected && !(await userMenager.IsInRoleAsync(user, role.Name)))
                {
                    await userMenager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userMenager.IsInRoleAsync(user, role.Name))
                {
                    await userMenager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userMenager.Users;
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userMenager.FindByIdAsync(id);
            var userRoles = await userMenager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                City = user.City,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Roles = userRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userMenager.FindByIdAsync(model.Id);
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;
                var result = await userMenager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddorRemoveUserFromRole(string userId)
        {
            ViewBag.userId = userId;
            var user = await userMenager.FindByIdAsync(userId);
            var model = new List<UserRoleViewModel>();
            foreach (var role in roleMenager.Roles.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    id = role.Id,
                    Name = role.Name,
                    IsSelected = false
                };
                if (await userMenager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddorRemoveUserFromRole(List<UserRoleViewModel> model, string userId)
        {
            var user = await userMenager.FindByIdAsync(userId);
            for(int i =0; model.Count >i; i++)
            {
                var role = await roleMenager.FindByIdAsync(model[1].id);
                if (model[i].IsSelected && !(await userMenager.IsInRoleAsync(user, role.Name)))
                {
                    await userMenager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userMenager.IsInRoleAsync(user, role.Name))
                {
                    await userMenager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }
            return RedirectToAction("EditUser", new { Id = userId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userMenager.FindByIdAsync(Id);
            var result = await userMenager.DeleteAsync(user);
            if(result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("ListUsers");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleMenager.FindByIdAsync(Id);
            var result = await roleMenager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("ListRoles");
        }
    }

}
