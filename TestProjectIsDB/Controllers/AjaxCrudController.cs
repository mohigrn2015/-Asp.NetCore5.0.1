using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using TestProjectIsDB.Data;
using TestProjectIsDB.Models.Classes;
using TestProjectIsDB.Models.PlayerViewModel;
using Microsoft.AspNetCore.Authorization;

namespace TestProjectIsDB.Controllers
{
    [Authorize]
    public class AjaxCrudController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AjaxCrudController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public ActionResult Index()
        {
            return View(_context.players.ToList());
        }
        [HttpGet]
        public JsonResult GetAllGetegory()
        {

            var dataList = _context.grades.Where(x => x.Status == 1).ToList();
            //var dataList = db.Categories.ToList();
            return Json(dataList);
        }
        [HttpGet]
        public JsonResult GetCategories()
        {
            //var dataList = _context.grades.ToList();
            var dataList = _context.grades.Select(x => new
            {
                GradeID = x.GradeID,
                GradeName = x.GradeName
            }).ToList();
            return Json(dataList);
        }

        //[HttpGet]
        //public ActionResult GetCategories()
        //{

        //    return Json(_context.grades.Select(x => new
        //    {
        //        GradeId = x.GradeID,
        //        Name = x.GradeName
        //    }).ToList());
        //}
        public ActionResult AddNewProduct()
        {
            return View(_context.players.ToList());
        }
        [HttpPost]
        public ActionResult SaveData(CreatePlayerModel item)
        {
            string uniqueFile = ProcessUploadFile(item);
            Player item1;
            if (item.PlayerID == 0)
            {
                item1 = new Player();
                item.ImageUrl = uniqueFile;
                item1.GradeID = item.GradeID;
                item1.DoB = item.DoB;
                item1.Name = item.Name;
                item1.Email = item.Email;
                item1.Phone = item.Phone;
                item1.Team = item.Team;
                item1.Salary = item.Salary;
                item1.ImageName = item.ImageName;
                item1.ImageUrl = item.ImageUrl;
                _context.players.Add(item1);
                _context.SaveChanges();
            }
            else
            {
                item1 = _context.players.SingleOrDefault(p => p.PlayerID == item.PlayerID);
                item.ImageUrl = uniqueFile;
                item1.PlayerID = item.PlayerID;
                item1.GradeID = item.GradeID;
                item1.DoB = item.DoB;
                item1.Name = item.Name;
                item1.Email = item.Email;
                item1.Phone = item.Phone;
                item1.Team = item.Team;
                item1.Salary = item.Salary;
                item1.ImageName = item.ImageName;
                item1.ImageUrl = item.ImageUrl;
                //db.players.Add(item);
                _context.SaveChanges();
            }
            var result = "Successfully Added";
            return Json(result);
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

        public JsonResult GetPlayerList()
        {
            var playerList = _context.players.Where(p => p.PlayerID > 0).Select(p => new CreatePlayerModel
            {
                PlayerID = p.PlayerID,
                Name = p.Name,
                Team = p.Team,
                DoB = p.DoB,
                Email = p.Email,
                Phone = p.Phone,
                Salary = p.Salary,
                ImageUrl = p.ImageUrl
            }).ToList();
            return Json(playerList);
        }
        public JsonResult GetPlayersById(int PlayerID)
        {
            Player obj = _context.players.Where(p => p.PlayerID == PlayerID).SingleOrDefault();
            string value = "";
            value = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }
        public PartialViewResult GetplayerDetails(int PlayerId)
        {
            Player obj = _context.players.Where(p => p.PlayerID == PlayerId).SingleOrDefault();
            PlayerListViewModel vObj = new PlayerListViewModel();
            vObj.PlayerID = obj.PlayerID;
            vObj.Name = obj.Name;
            vObj.Team = obj.Team;
            vObj.DoB = obj.DoB;
            vObj.Email = obj.Email;
            vObj.Phone = obj.Phone;
            vObj.Salary = obj.Salary;
            vObj.ImageUrl = obj.ImageUrl;
            //vObj.GradeName = obj.Grade.GradeName;
            return PartialView("_PlayerDetailsPartial", vObj);
        }
        public ActionResult deleteRecord(int Id)
        {
            bool result = false;
            Player obj = _context.players.Where(p => p.PlayerID == Id).SingleOrDefault();
            if (obj != null)
            {
                _context.players.Remove(obj);
                _context.SaveChanges();
                result = true;
            }
            return View();
            //string value = "";
            //value = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});
            //return Json(value, JsonRequestBehavior.AllowGet);
        }
        //private string ProcessUploadFile(CreatePlayerModel viewobj)
        //{
        //    string uniqueFileName = null;
        //    var files = HttpContext.Request.Form.Files;
        //    foreach (var image in files)
        //    {
        //        if (image != null && image.Length > 0)
        //        {
        //            var file = image;
        //            var uploadFile = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
        //            if (file.Length > 0)
        //            {
        //                var fileName = file.FileName;
        //                using (var fileStream = new FileStream(Path.Combine(uploadFile, fileName), FileMode.Create))
        //                {
        //                    file.CopyTo(fileStream);
        //                    uniqueFileName = fileName;
        //                }
        //            }

        //        }
        //    }

        //    return uniqueFileName;
        //}
    }
}
