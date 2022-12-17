using LIbrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIbrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Book Book { get; set; }
        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id == null)
            {
                //create
                return View(Book);

            }
            //update
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if(Book == null)
            {
                return NotFound();
            }
            
            return View(Book);
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            if(ModelState.IsValid)
            {
                if(Book.Id==0)
                {
                    //create
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _db.Books.ToListAsync();
            return Json(new { data });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = _db.Books.FirstOrDefault(u => u.Id == id);
            if (bookFromDb == null) return Json(new { success = false, message = "Error while deleting" });
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
