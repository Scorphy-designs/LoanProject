using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanProcessingSystem.Model;

namespace LoanProcessingSystem.Controllers
{
    public class EnquiryController : Controller
    {
        private MortgageDbEntities db = new MortgageDbEntities();

        // GET: Enquiry
        public ActionResult Index()
        {
            return View(db.EnquiryTables.ToList());
        }

        // GET: Enquiry/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            if (enquiryTable == null)
            {
                return HttpNotFound();
            }
            return View(enquiryTable);
        }

        // GET: Enquiry/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Enquiry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnquiryId,EmailId,PhoneNo,PropertyType,Date,Message")] EnquiryTable enquiryTable)
        {
            if (ModelState.IsValid)
            {
                enquiryTable.Date = DateTime.Now.Date;
                db.EnquiryTables.Add(enquiryTable);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(enquiryTable);
        }

        // GET: Enquiry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            if (enquiryTable == null)
            {
                return HttpNotFound();
            }
            return View(enquiryTable);
        }

        // POST: Enquiry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnquiryId,EmailId,PhoneNo,PropertyType,Date,Message")] EnquiryTable enquiryTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enquiryTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            return View(enquiryTable);
        }

        // GET: Enquiry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            if (enquiryTable == null)
            {
                return HttpNotFound();
            }
            return View(enquiryTable);
        }

        // POST: Enquiry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            db.EnquiryTables.Remove(enquiryTable);
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
        public ActionResult lastMessage()
        {
            ViewBag.Message = "You Sucesfully Submit Query";
            return View();
        }
    }
}