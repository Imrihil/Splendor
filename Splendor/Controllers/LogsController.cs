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
using PagedList;

namespace Splendor.Controllers
{
    [Authorize(Users = UserRoles.UserAdmin)]
    public class LogsController : Controller
    {
        private SplendorDbContext db = new SplendorDbContext();

        // GET: Logs
        public ActionResult Index(string searchType, string searchUserName, string searchGameName, string searchMessage, int? page)
        {
            ViewBag.SearchType = searchType;
            ViewBag.SearchUserName = searchUserName;
            ViewBag.SearchGameName = searchGameName;
            ViewBag.SearchMessage = searchMessage;

            IEnumerable<Log> logs = db.Logs;

            if (!string.IsNullOrEmpty(searchType))
            {
                logs = logs.Where(x => x.Type.ToLower().Contains(searchType.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchUserName))
            {
                logs = logs.Where(x => x.UserName.ToLower().Contains(searchUserName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchGameName))
            {
                logs = logs.Where(x => x.Game == null ? false : x.Game.Name.ToLower().Contains(searchGameName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchMessage))
            {
                logs = logs.Where(x => searchMessage.ToLower().Split('_').All(y => x.Message.Contains(y.ToLower())));
            }

            IOrderedEnumerable<Log> orderedLogs = logs.OrderByDescending(x => x.Time);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.PageSize = pageSize;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexTable", orderedLogs.ToPagedList(pageNumber, pageSize));
            }

            return View(orderedLogs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // GET: Logs/Create
        public ActionResult Create()
        {
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name");
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Time,UserName,Type,GameId,Message")] Log log)
        {
            if (ModelState.IsValid)
            {
                db.Logs.Add(log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", log.GameId);
            return View(log);
        }

        // GET: Logs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", log.GameId);
            return View(log);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Time,UserName,Type,GameId,Message")] Log log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", log.GameId);
            return View(log);
        }

        // GET: Logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Log log = db.Logs.Find(id);
            db.Logs.Remove(log);
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
