using Microsoft.AspNetCore.Mvc;
using PhotoApp.Services.Models.User;
using PhotoApp.Services.UserService;
using PhotoApp.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IUserService userService;

        public RolesController(IUserService userService
                                )
        {
            this.userService = userService;
        }

        public async Task<IActionResult> All()
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            editUserViewModel.IsAdmin = true;
            editUserViewModel.IsMember = false;
            editUserViewModel.IsModerator = false;
              

            return View();
        }

        public async Task<IActionResult> User(string id)
        {
            var user = await userService.GetUserById(id);

            EditUserViewModel viewModel = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Birthday = user.Birthday,
                CreatedOn = user.RegisterDate,
                Email = user.Email,
                Id = user.Id,
                Phone = user.PhoneNumber,
                IsAdmin = user.IsAdmin,
                IsMember = user.IsMember,
                IsModerator = user.IsModerator
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            UserServiceModel userServiceModel = new UserServiceModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Birthday = model.Birthday,
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                IsMember = model.IsMember,
                IsModerator = model.IsModerator,
                PhoneNumber = model.Phone,
            };

           await userService.EditUser(userServiceModel);

            return RedirectToAction("/Admin/Roles/Edit/" + model.Id);
        }
    }
}
