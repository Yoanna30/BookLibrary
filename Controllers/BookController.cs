using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using BookLibrary.Entities;
using BookLibrary.ExtentionMethods;
using BookLibrary.Models.Books;
using BookLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            BookLibraryDbContext context = new BookLibraryDbContext();

            IndexVM model = new IndexVM();
            model.bookCollection = context.Books.ToList();
            model.Summary = context.Summaries.ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();

            Book currentB = context.Books.Where(l => l.Id == id).FirstOrDefault();

            if (currentB == null)
            {
                return RedirectToAction("Index", "Book");
            }

            DetailsVM item = new DetailsVM();
            item.Heading = currentB.Heading;
            item.ReleaseDate = currentB.ReleaseDate;
            item.Pages = currentB.Pages;
            item.Quantity = currentB.Quantity;
            if (currentB.SummaryId != null)
            {
                item.Description = currentB.Summary.Content;
            }

            item.Genre = currentB.Genre.Name;
            item.BookId = currentB.Id;
            if (currentB.ImageId != null)
            {
                item.ImageUrl = currentB.Image.Url;
            }

            item.CountOfLikes = context.Likes.Where(l => l.BookId == currentB.Id).ToList().Count;
            item.Comments = context.Comments.Where(l => l.BookId == currentB.Id).ToList();
            item.Users = context.Users.ToList();


            return View(item);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            BookLibraryDbContext context = new BookLibraryDbContext();

            Book current = context.Books.Where(l => l.Id == id).FirstOrDefault();

            if (current == null)
            {
                return RedirectToAction("Index", "Book");
            }

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                return RedirectToAction("Index", "Book");
            }

            EditVM model = new EditVM();
            model.GenreCollection = context.Genres.ToList();
            model.Summary = current.Summary;
            model.Author = current.Author;
            model.Heading = current.Heading;
            model.Pages = current.Pages;
            model.Quantity = current.Quantity;
            model.ReleaseDate = current.ReleaseDate;
            model.Image = current.Image;

            return View(model);

        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            Book item = new Book();
            item.Id = model.Id;
            item.Summary = model.Summary;
            item.Genre = model.Genre;
            item.Heading = model.Heading;
            item.Author = model.Author;
            item.Pages = model.Pages;
            item.Quantity = model.Quantity;
            item.ReleaseDate = model.ReleaseDate;
            item.GenreId = model.GenreID;
            item.Image = model.Image;

            context.Books.Update(item);
            context.SaveChanges();
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public IActionResult Create()
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                return RedirectToAction("Index", "Book");
            }

            CreateVM model = new CreateVM();
            model.GenreCollection = context.Genres.ToList();

            return View(model);

        }


        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                ModelState.AddModelError("summaryError", "You don't have permission to add books!");
                return View(model);
            }

            BookLibraryDbContext context = new BookLibraryDbContext();

            Book item = new Book();
            item.Summary = model.Summary; 
            item.Genre = model.Genre; 
            item.Heading = model.Heading; 
            item.Author = model.Author;
            item.Pages = model.Pages;
            item.Quantity = model.Quantity;
            item.ReleaseDate = model.ReleaseDate;
            item.GenreId = model.GenreID; 
            item.Image = model.Image; 

            context.Books.Add(item);
            context.SaveChanges();
            return RedirectToAction("Index", "Book");


        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            Book current = context.Books.Where(l => l.Id == id).FirstOrDefault();

            if (current == null)
            {
                return RedirectToAction("Index", "Book");
            }

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                return RedirectToAction("Index", "Book");
            }

            DeleteVM model = new DeleteVM();
            model.Id = current.Id;
            model.UserType = loggedUser.TypeOfUser;

            return View(model);

        }

        [HttpPost]
        public IActionResult Delete(DeleteVM model)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            Book item = context.Books.Find(model.Id);

            context.Books.Remove(item);
            context.SaveChanges();
            return RedirectToAction("Index", "Book");
        }


        [HttpPost]
        public IActionResult Like (int BookId)
        {
            BookLibraryDbContext context = new BookLibraryDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            if (loggedUser != null)
            {
                List<Like> userLikes =
                    context.Likes.Where(l => l.UserId == loggedUser.Id && l.BookId == BookId).ToList();
                if (userLikes.Count == 0)
                {
                    Like item = new Like();
                    item.BookId = BookId;
                    item.UserId = loggedUser.Id;
                    context.Likes.Add(item);
                    context.SaveChanges();
                }
            }

            string link = "/Book/Details?id=" + BookId;
            return Redirect(link);

        }

        [HttpPost]
        public IActionResult Comment(DetailsVM model)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            BookLibraryDbContext context = new BookLibraryDbContext();

            if (loggedUser != null && model.ContentComment != null)
            {
                Comment comment = new Comment();
                comment.UserId = loggedUser.Id;
                comment.BookId = model.BookId;
                comment.Content = model.ContentComment;
                context.Comments.Add(comment);
                context.SaveChanges();
            }
            string link = "/Book/Details?id=" + model.BookId;
            return Redirect(link);
        }
    }
}