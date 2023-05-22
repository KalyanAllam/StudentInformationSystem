using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIS.Models;
using ClosedXML;
using ClosedXML.Excel;
using System.IO;

namespace SIS.Controllers
{
    public class StudentsController : Controller
    {
        private ContosoUniversityDataEntities2 db = new ContosoUniversityDataEntities2();

        // GET: Students
        public ActionResult Index(string searchString)
        {
            var students = db.Students.Include(s => s.Class).Include(s => s.Section);
            students = students.OrderBy(s => s.RollNo);
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }


            students = students.OrderBy(student => student.RollNo);
                return View(students.ToList());
        }


        [HttpPost]
        public FileResult Export()
        {

            DataTable dt = new DataTable("Student");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("FirstName"),
                                            new DataColumn("LastName"),
                                            new DataColumn("PhoneNo"),
                                            new DataColumn("Email"),
                                            new DataColumn("ClassName"),
                                            new DataColumn("ClassSectionName"),
                                              new DataColumn("RollNo")


            });

            var students = from student in db.Students
                           select student;

            foreach (var student in students)
            {
                dt.Rows.Add(student.FirstName, student.LastName, student.PhoneNo, student.Email, student.Class.ClassName, student.Section.ClassSectionName,student.RollNo);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Student.xlsx");
                }
            }
        }


        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName");
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "ClassSectionName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,ClassID,LastName,FirstName,PhoneNo,Email,SectionID,RollNo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", student.ClassID);
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "ClassSectionName", student.SectionID);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", student.ClassID);
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "ClassSectionName", student.SectionID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,ClassID,LastName,FirstName,PhoneNo,Email,SectionID,RollNo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", student.ClassID);
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "ClassSectionName", student.SectionID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
