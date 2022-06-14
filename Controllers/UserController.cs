using BookLibrary.Entities;
using BookLibrary.ExtentionMethods;
using BookLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using BookLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            BookLibraryDbContext context = new BookLibraryDbContext();

            IndexVM model = new IndexVM();
            model.allUsers = context.Users.ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            CreateVM model = new CreateVM();
            model.types = new List<string> { "moderator", "admin", "normal" };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();

            User user = new User();
            user.Username = model.Username;
            user.Password = model.Password;
            user.FirstName = model.Firstname;
            user.LastName = model.LastName;
            user.TypeOfUser = model.TypeOfUser;
            context.Users.Add(user);
            context.SaveChanges();

            //HttpContext.Session.SetObject<User>("addedUser", user);
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User logggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            BookLibraryDbContext context = new BookLibraryDbContext();

            User current = context.Users.Where(l => l.Id == id).FirstOrDefault();

            /*if (current == null)
            {
                return RedirectToAction("Index", "User");
            }*/

            EditVM model = new EditVM();
            model.Username = current.Username;
            model.Password = current.Password;
            model.TypeOfUser = current.TypeOfUser;
            model.Firstname = current.FirstName;
            model.LastName = current.LastName;
            model.types = new List<string> { "moderator", "admin", "normal" };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model) {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User user = new User();
            user.Id = model.Id;
            user.Username = model.Username;
            user.Password = model.Password;
            user.TypeOfUser = model.TypeOfUser;
            user.FirstName = model.Firstname;
            user.LastName = model.LastName;
            context.Users.Update(user);
            context.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            User current = context.Users.Where(user => user.Id == id).FirstOrDefault();

            DeleteVm model = new DeleteVm();
            model.Id = current.Id;


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteVm model) {
            BookLibraryDbContext context = new BookLibraryDbContext();

            User user = context.Users.Find(model.Id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index","User");
        }

        [HttpGet]
        public IActionResult UserInfo(int id)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            //User current = context.Users.Where(user => user.Id == id).FirstOrDefault();
            UserInfoVM model = new UserInfoVM();

            model.Username = loggedUser.Username;
            model.Password = loggedUser.Password;
            model.Firstname = loggedUser.FirstName;
            model.LastName = loggedUser.LastName;
            return View(model);
        }

        [HttpPost]
        public IActionResult UserInfo(UserInfoVM model)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            loggedUser.Username = model.Username;
            loggedUser.Password = model.Password;
            loggedUser.FirstName = model.Firstname;
            loggedUser.LastName = model.LastName;

            context.Users.Update(loggedUser);
            context.SaveChanges();
            return RedirectToAction("Login", "Home");
        }
    }
}