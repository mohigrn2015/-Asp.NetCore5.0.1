using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestProjectIsDB.Data;
using TestProjectIsDB.Models.Classes;

namespace TestProjectIsDB.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name");
            return View(await _context.Categories.ToListAsync());
        }

        public ActionResult GetCategoryWiseItems(long? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewData["id"] = id;
            List<Item> items = _context.Items.Where(e => e.CategoryID == id).ToList();

            if (items == null)
            {
                //return HttpNotFound();
            }

            return PartialView("CategoryWiseItems", items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,Items")] Category category, IFormFile[] Image)
        {
            //string path = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        if (category.Items.Count == Image.Count())
                        {
                            //path = ProcessUploadFile();
                            for (int i = 0; i < category.Items.Count; i++)
                            {
                                //string picture = System.IO.Path.GetFileName(Image[i].FileName);
                                //path = System.IO.Path.Combine();
                                string picture = System.IO.Path.GetFileName(Image[i].FileName);
                                var file = picture;
                                var uploadFile = Path.Combine(_webHostEnvironment.WebRootPath, "Images", picture);

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    Image[i].CopyTo(ms);
                                    category.Items[i].Image = ms.GetBuffer().ToString();
                                }
                            }
                        }
                        _context.Categories.Add(category);
                        _context.SaveChanges();
                        TempData["id"] = category.ID;
                        return RedirectToAction("Index");
                    }
                }
                return View(category);
            }
            catch (Exception)
            {
                return View(category);
            }
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CategoryID = new SelectList(_context.Categories.Include("Items"), "ID", "Name", id);
            Category category = await _context.Categories.Include(e=>e.Items).SingleAsync(e=>e.ID==id);
            
            if (category == null)
            {
                //return HttpNotFound();
            }
            //ViewBag.CategoryID = new SelectList(_context.Categories.Include("Items"), "ID", "Name", id);
            //Thread.Sleep(5000);
            return PartialView(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("ID,Name,Items")] Category category, IFormFile[] file)
        {
            string path = "";
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    for (int i = 0; i < category.Items.Count; i++)
                    {
                        if (file[i] != null)
                        {
                            // To save a image to a folder
                            //string picture = System.IO.Path.GetFileName(file[i].FileName);
                            path = ProcessUploadFile();
                            //file[i].SaveAs(path);

                            // To store as byte[] in a Table of Database
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file[i].CopyTo(ms);
                                category.Items[i].Image = ms.GetBuffer().ToString();
                            }
                        }
                    }
                }
                _context.Entry(category).State = EntityState.Modified;
                foreach (Item item in category.Items)
                {
                    _context.Entry(item).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                TempData["id"] = category.ID;
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CategoryID = new SelectList(_context.Categories.Include("Items"), "ID", "Name", id);
            Category category = await _context.Categories.Include(e=>e.Items).SingleAsync(e=>e.ID==id);
            if (category == null)
            {
                //return HttpNotFound();
            }
            return PartialView(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Category category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }





        private string ProcessUploadFile()
        {
            string uniqueFileName = null;
            var files = HttpContext.Request.Form.Files;
            foreach (var image in files)
            {
                if (image != null && image.Length > 0)
                {
                    var file = image;
                    var uploadFile = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    if (file.Length > 0)
                    {
                        var fileName = file.FileName;
                        using (var fileStream = new FileStream(Path.Combine(uploadFile, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            uniqueFileName = fileName;
                        }
                    }

                }
            }

            return uniqueFileName;
        }
    }
}
