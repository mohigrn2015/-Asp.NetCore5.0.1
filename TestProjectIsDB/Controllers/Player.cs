using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestProjectIsDB.Attributes;
using TestProjectIsDB.Data;
using TestProjectIsDB.Models.Classes;
using TestProjectIsDB.Models.PlayerViewModel;

namespace TestProjectIsDB.Controllers
{
    [Authorize]
    [Controller]
    public class PlayerController : Controller 
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public PlayerController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        [CustomAuthorize("Player", "Index")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(string SearchString, string CurrentFilter, string sortOrder, int? Page)
        {
            ViewData["Create"] = RolesForMenu.GetMenu(User.Identity.Name, "Player", "Create");
            ViewData["Edit"] = RolesForMenu.GetMenu(User.Identity.Name, "Player", "Edit");
            ViewData["Delete"] = RolesForMenu.GetMenu(User.Identity.Name, "Player", "Delete");
            var applicationDbContext = _context.players.Include(c => c.Name);

            ViewBag.SortNameParam = string.IsNullOrEmpty(sortOrder) ? "name_des" : "";
            ViewBag.Salary = string.IsNullOrEmpty(sortOrder) ? "salary_des" : "";
            if (SearchString != null)
            {
                Page = 1;
            }
            else
            {
                SearchString = CurrentFilter;
            }
            ViewBag.CurrentFilter = SearchString;


            List<PlayerListViewModel> playerList = _context.players.Select(p => new PlayerListViewModel
            {
                PlayerID = p.PlayerID,
                Name = p.Name,
                DoB = p.DoB,
                Team = p.Team,
                Email = p.Email,
                Phone = p.Phone,
                Salary = p.Salary,
                ImageName = p.ImageName,
                ImageUrl = p.ImageUrl,
                GradeID = p.GradeID,
                GradeName = p.Grade.GradeName,
            }).ToList();            

            if (!string.IsNullOrEmpty(SearchString))
            {
                playerList = playerList.Where(n => n.Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_des":
                    playerList = playerList.OrderByDescending(n => n.Name).ToList();
                    break;
                case "salary_des":
                    playerList = playerList.OrderByDescending(n => n.Salary).ToList();
                    break;
                default:
                    playerList = playerList.OrderBy(n => n.Name).ToList();
                    break;
            }
            int PageSize = 100;
            int PageNumber = (Page ?? 1);
            return View("Index", playerList.ToPagedList(PageNumber, PageSize));           
            
        }
        [CustomAuthorize("Player", "Create")]
        [HttpGet]        
        public ActionResult Create()
        {
            CreatePlayerModel crObj = new CreatePlayerModel();
            crObj.gradeList = _context.grades.ToList();
            return View(crObj);
        }
        [CustomAuthorize("Player", "AddOrEdit")]
        //[HttpPost, ActionName("AddOrEdit")]
        //[ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(CreatePlayerModel viewObj)
        {
            var result = false;

            string uniqueFile = ProcessUploadFile(viewObj);
            Player playerObj = new Player();
            playerObj.Name = viewObj.Name;
            playerObj.DoB = viewObj.DoB;
            playerObj.Team = viewObj.Team;
            playerObj.Email = viewObj.Email;
            playerObj.Phone = viewObj.Phone;
            playerObj.Salary = viewObj.Salary;
            playerObj.GradeID = viewObj.GradeID;            
            playerObj.ImageUrl = uniqueFile;
            if (ModelState.IsValid)
            {
                if (viewObj.PlayerID == 0)
                {
                    _context.players.Add(playerObj);
                    _context.SaveChanges();
                    result = true;
                }
                else
                {
                    playerObj.PlayerID = viewObj.PlayerID;
                    _context.Entry(playerObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = true;
                }
            }
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (viewObj.PlayerID == 0)
                {
                    CreatePlayerModel crObj = new CreatePlayerModel();
                    crObj.gradeList = _context.grades.ToList();
                    return View("Create", crObj);
                }
                else
                {
                    CreatePlayerModel crObj = new CreatePlayerModel();
                    crObj.gradeList = _context.grades.ToList();
                    return View("Edit", crObj);
                }
            }

        }
        [HttpGet]
        [CustomAuthorize("Player", "Edit")]
        public ActionResult Edit(int id)
        {

            Player playerObj = _context.players.SingleOrDefault(p => p.PlayerID == id);
            CreatePlayerModel viewObj = new CreatePlayerModel();
            if (playerObj != null)
            {
                viewObj.PlayerID = playerObj.PlayerID;
                viewObj.Name = playerObj.Name;
                viewObj.DoB = playerObj.DoB;
                viewObj.Team = playerObj.Team;
                viewObj.Email = playerObj.Email;
                viewObj.Phone = playerObj.Phone;
                viewObj.ImageName = playerObj.ImageName;
                viewObj.Salary = playerObj.Salary;
                viewObj.GradeID = playerObj.GradeID;
                viewObj.gradeList = _context.grades.ToList();
                viewObj.ImageUrl = playerObj.ImageUrl;
            }
            return View(viewObj);
        }

        [HttpGet]
        [CustomAuthorize("Player", "Delete")]
        public ActionResult Delete(int id)
        {
            Player playerObj = _context.players.SingleOrDefault(p => p.PlayerID == id);
            CreatePlayerModel viewObj = new CreatePlayerModel();
            if (playerObj != null)
            {
                viewObj.PlayerID = playerObj.PlayerID;
                viewObj.Name = playerObj.Name;
                viewObj.DoB = playerObj.DoB;
                viewObj.Team = playerObj.Team;
                viewObj.Email = playerObj.Email;
                viewObj.Phone = playerObj.Phone;
                viewObj.Salary = playerObj.Salary;
                viewObj.GradeID = playerObj.GradeID;
                viewObj.ImageUrl = playerObj.ImageUrl;
            }

            return View(viewObj);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            Player playerObj = _context.players.SingleOrDefault(p => p.PlayerID == id);
            if (playerObj != null)
            {
                _context.players.Remove(playerObj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(playerObj);
        }
        [CustomAuthorize("Player", "Details")]
        [HttpGet]
        public ActionResult Details(int PlayerID)
        {
            Player playerObj = _context.players.SingleOrDefault(p => p.PlayerID == PlayerID);
            PlayerListViewModel viewObj = new PlayerListViewModel();
            viewObj.PlayerID = playerObj.PlayerID;
            viewObj.Name = playerObj.Name;
            viewObj.DoB = playerObj.DoB;
            viewObj.Team = playerObj.Team;
            viewObj.Email = playerObj.Email;
            viewObj.Phone = playerObj.Phone;
            viewObj.Salary = playerObj.Salary;
            viewObj.GradeID = playerObj.GradeID;
            //viewObj.GradeName = playerObj.Grade.GradeName;
            viewObj.ImageUrl = playerObj.ImageUrl;
            return PartialView("_DetailsRecord", viewObj);
        }
        private string ProcessUploadFile(CreatePlayerModel viewobj)
        {
            string uniqueFileName = null;
            var files = HttpContext.Request.Form.Files;
            foreach (var image in files)
            {
                if (image != null && image.Length > 0)
                {
                    var file = image;
                    var uploadFile = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
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
