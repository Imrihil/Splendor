using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Splendor.DAL;
using Splendor.Models;

namespace Splendor.Controllers
{
    [Authorize(Users = UserRoles.UserAdmin)]
    public class AristocratsController : Controller
    {
        private SplendorDbContext db = new SplendorDbContext();

        // GET: Aristocrats
        public ActionResult Index()
        {
            return View(db.Aristocrats.ToList());
        }

        // GET: Aristocrats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aristocrat aristocrat = db.Aristocrats.Find(id);
            if (aristocrat == null)
            {
                return HttpNotFound();
            }
            return View(aristocrat);
        }

        // GET: Aristocrats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Aristocrats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Points,RequireGreen,RequireWhite,RequireBlue,RequireBlack,RequireRed,ImagePath")] Aristocrat aristocrat)
        {
            if (ModelState.IsValid)
            {
                db.Aristocrats.Add(aristocrat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aristocrat);
        }

        // GET: Aristocrats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aristocrat aristocrat = db.Aristocrats.Find(id);
            if (aristocrat == null)
            {
                return HttpNotFound();
            }
            return View(aristocrat);
        }

        // POST: Aristocrats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Points,RequireGreen,RequireWhite,RequireBlue,RequireBlack,RequireRed,ImagePath")] Aristocrat aristocrat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aristocrat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aristocrat);
        }

        // GET: Aristocrats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aristocrat aristocrat = db.Aristocrats.Find(id);
            if (aristocrat == null)
            {
                return HttpNotFound();
            }
            return View(aristocrat);
        }

        // POST: Aristocrats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aristocrat aristocrat = db.Aristocrats.Find(id);
            db.Aristocrats.Remove(aristocrat);
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
