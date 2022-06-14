using BookLibrary.ActionFilters;
using BookLibrary.Entities;
using BookLibrary.ExtentionMethods;
using BookLibrary.Models;
using BookLibrary.Models.Home;
using BookLibrary.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            User loggedUser = context.Users.Where(m => m.Username == model.Username &&
                                                       m.Password == model.Password)
                                                        .FirstOrDefault();

            if(loggedUser == null || (loggedUser.Password != model.Password) || (loggedUser.Username != model.Username))
            {
                this.ModelState.AddModelError("authError", "Invalid password or username!");
            }
            else
            {
                HttpContext.Session.SetObject("loggedUser", loggedUser);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [AuthenticationFilterAttrubutes]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {

            BookLibraryDbContext context = new BookLibraryDbContext();
            User newUser = new User();
            newUser.FirstName = model.FirstName;
            newUser.LastName = model.LastName;
            newUser.Username = model.Username;
            newUser.Password = model.Password;
            context.Add(newUser);
            context.SaveChanges();
            return RedirectToAction("Login", "Home");
        }

    }
}