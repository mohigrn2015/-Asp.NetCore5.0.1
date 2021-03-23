using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestProjectIsDB.Data;
using TestProjectIsDB.Models.Classes;

namespace TestProjectIsDB.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IWebHostEnvironment _webHostEnvironment { get; }

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public ActionResult Create(long? id)
        {
            if (id == null)
            {
                ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name");
            }
            else
            {
                ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name", id);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ID,Name,Description,EntryDate,Quantity,CategoryID")] Item item, IFormFile file)
        {
            //string path = "";
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // To save a image to a folder
                    string picture = System.IO.Path.GetFileName(file.FileName);
                    //path = ProcessUploadFile();
                    //file.SaveAs(path);

                    // To store as byte[] in a Table of Database
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        item.Image = ms.GetBuffer().ToString();
                    }
                    _context.Items.Add(item);
                    await _context.SaveChangesAsync();
                    TempData["id"] = item.CategoryID;
                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name", item.CategoryID);
                    return PartialView(item);
                }
            }
            ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name", item.CategoryID);
            return PartialView(item);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                //return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name", item.CategoryID);
            return PartialView(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("ID,Name,Description,Image,EntryDate,Quantity,CategoryID")] Item item, IFormFile file)
        {
            string path = "";
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // To save a image to a folder
                    //string picture = System.IO.Path.GetFileName(file.FileName);
                    path = ProcessUploadFile();
                    //file.SaveAs(path);

                    // To store as byte[] in a Table of Database
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        item.Image = ms.GetBuffer().ToString();
                    }
                }
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["id"] = item.CategoryID;
                return RedirectToAction("Index", "Categories");
            }
            ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name", item.CategoryID);
            return PartialView(item);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                //return HttpNotFound();
            }
            return PartialView(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Item item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            TempData["id"] = item.CategoryID;
            return RedirectToAction("Index", "Categories");
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
