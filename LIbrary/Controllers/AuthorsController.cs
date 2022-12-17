using LIbrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIbrary.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Author Author { get; set; }
        public AuthorsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Author.Id == 0)
                {
                    //create
                    _db.Authors.Add(Author);
                }
                else
                {
                    _db.Authors.Update(Author);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Author);
        }
        public IActionResult Upsert(int? id)
        {
            Author = new Author();
            if (id == null)
            {
                //create
                return View(Author);
            }
            //update
            Author = _db.Authors.FirstOrDefault(u => u.Id == id);
            if (Author == null)
            {
                return NotFound();
            }

            return View(Author);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var authorFromDb = _db.Authors.FirstOrDefault(u => u.Id == id);
            if (authorFromDb == null) return Json(new { success = false, message = "Error while deleting" });
            _db.Authors.Remove(authorFromDb);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Authors.ToListAsync() });
        }

        /*[HttpGet]
        public async Task<IActionResult> LinkFirstAuthorToFirstBook()
        {
            Author author = _db.Authors.FirstOrDefault();
            Book book = _db.Books.FirstOrDefault();
            book.Author = author;
            _db.SaveChanges();

            return Json(new { data = await _db.Authors.ToListAsync() });
        }*/

    }
}
