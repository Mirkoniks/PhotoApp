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

        [HttpGet]
        public async Task<IActionResult> All()
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            editUserViewModel.IsAdmin = true;
            editUserViewModel.IsMember = false;
            editUserViewModel.IsModerator = false;


            return View();
        }

        [HttpGet]
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

        [HttpPatch]
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

            return Redirect("/Admin/Roles/User/" + model.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchModel model)
        {
            if (!model.IsValid)
            {
                ModelState.AddModelError("Username", "Username is inavalid");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchConfirm(SearchModel model)
        {
            if (await userService.CheckIfUsernameIsValid(model.Username))
            {
                string userId = await userService.GetUserIdByUsername(model.Username);

                return Redirect("/Admin/Roles/User/" + userId);
            }


            model.IsValid = false;

            return RedirectToAction("Search", model);
        }

        [HttpGet]
        public async Task<IActionResult> Admins()
        {
            UsersVIewModel usersVIewModel = new UsersVIewModel();
            List<UserViewModel> users = new List<UserViewModel>();

            var usersDb = await userService.GetAllAdmins();

            foreach (var item in usersDb.Users)
            {
                UserViewModel user = new UserViewModel()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName,
                    Role = item.Role
                };

                users.Add(user);
            }

            usersVIewModel.Users = users;

            return View(usersVIewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Moderators()
        {
            UsersVIewModel usersVIewModel = new UsersVIewModel();
            List<UserViewModel> users = new List<UserViewModel>();

            var usersDb = await userService.GetAllModerators();

            foreach (var item in usersDb.Users)
            {
                UserViewModel user = new UserViewModel()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName,
                    Role = item.Role
                };

                users.Add(user);
            }

            usersVIewModel.Users = users;

            return View(usersVIewModel);
        }
    }
}
