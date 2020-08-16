using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty] // used becuase we do not want to retrive the book entry every time so we binded book with the database
        public Book Book { get; set; }

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        // The upsert will be used in two case either to insert new entry so no id requried or edit so the ID is not required , 
        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id==null)
            {
                // create request
                return View(Book);

            }
            // update case
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book==null)
            {
                return NotFound();

            }
            return View(Book);
        }


        // to do post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if(Book.Id==0)
                {
                    // create 
                    _db.Books.Add(Book);

                }
                else 
                {
                    // update 
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
            return Json(new { data = await _db.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }



        #endregion
    }
}
