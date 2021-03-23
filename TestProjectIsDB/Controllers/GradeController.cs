using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjectIsDB.Data;
using TestProjectIsDB.Models.Classes;
using TestProjectIsDB.Models.PlayerViewModel;

namespace TestProjectIsDB.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult GradeList(int pageNumber = 1)
        {
            List<Grade> gradeList = _context.grades.ToList();
            return View(gradeList);
        }
        [HttpGet]
        public ActionResult AddGrade()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddGrade(CreateGradeViewModel viewobj)
        {
            Grade gradeObj = new Grade();
            gradeObj.GradeName = viewobj.GradeName;
            _context.grades.Add(gradeObj);
            _context.SaveChanges();
            RedirectToAction("GradeList");
            return RedirectToAction("GradeList");
        }
        [HttpGet]
        public ActionResult EditGrade(int id)
        {
            Grade gradeObj = _context.grades.SingleOrDefault(g => g.GradeID == id);
            CreateGradeViewModel gradeObj2 = new CreateGradeViewModel();
            if (gradeObj != null)
            {
                // gradeObj = new Grade();
                gradeObj2.GradeName = gradeObj.GradeName;
            }

            return View(gradeObj);
        }
        [HttpPost]
        public ActionResult EditGrade(CreateGradeViewModel viewObj)
        {
            Grade gradeObj = new Grade();
            gradeObj.GradeName = viewObj.GradeName;
            gradeObj.GradeID = viewObj.GradeID;
            _context.Entry(gradeObj).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("GradeList");
        }
        [HttpGet, HttpPost]
        public ActionResult DeleteGrate(int id)
        {
            Grade gradeobj = _context.grades.SingleOrDefault(g => g.GradeID == id);
            if (gradeobj != null)
            {
                _context.grades.Remove(gradeobj);
                _context.SaveChanges();
                return RedirectToAction("GradeList");
            }
            return View(gradeobj);
        }
    }
}
