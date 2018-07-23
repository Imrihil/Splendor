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
    public class AristocratInGamesController : Controller
    {
        private SplendorDbContext db = new SplendorDbContext();

        // GET: AristocratInGames
        public ActionResult Index()
        {
            var aristocratInGames = db.AristocratInGames.Include(a => a.Aristocrat).Include(a => a.PlayerInGame);
            return View(aristocratInGames.ToList());
        }

        // GET: AristocratInGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AristocratInGame aristocratInGame = db.AristocratInGames.Find(id);
            if (aristocratInGame == null)
            {
                return HttpNotFound();
            }
            return View(aristocratInGame);
        }

        // GET: AristocratInGames/Create
        public ActionResult Create()
        {
            ViewBag.AristocratId = new SelectList(db.Aristocrats, "Id", "ImagePath");
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id");
            return View();
        }

        // POST: AristocratInGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AristocratId,PlayerInGameId")] AristocratInGame aristocratInGame)
        {
            if (ModelState.IsValid)
            {
                db.AristocratInGames.Add(aristocratInGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AristocratId = new SelectList(db.Aristocrats, "Id", "ImagePath", aristocratInGame.AristocratId);
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id", aristocratInGame.PlayerInGameId);
            return View(aristocratInGame);
        }

        // GET: AristocratInGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AristocratInGame aristocratInGame = db.AristocratInGames.Find(id);
            if (aristocratInGame == null)
            {
                return HttpNotFound();
            }
            ViewBag.AristocratId = new SelectList(db.Aristocrats, "Id", "ImagePath", aristocratInGame.AristocratId);
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id", aristocratInGame.PlayerInGameId);
            return View(aristocratInGame);
        }

        // POST: AristocratInGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AristocratId,PlayerInGameId")] AristocratInGame aristocratInGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aristocratInGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AristocratId = new SelectList(db.Aristocrats, "Id", "ImagePath", aristocratInGame.AristocratId);
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id", aristocratInGame.PlayerInGameId);
            return View(aristocratInGame);
        }

        // GET: AristocratInGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AristocratInGame aristocratInGame = db.AristocratInGames.Find(id);
            if (aristocratInGame == null)
            {
                return HttpNotFound();
            }
            return View(aristocratInGame);
        }

        // POST: AristocratInGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AristocratInGame aristocratInGame = db.AristocratInGames.Find(id);
            db.AristocratInGames.Remove(aristocratInGame);
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
