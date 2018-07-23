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
    public class PlayerInGamesController : Controller
    {
        private SplendorDbContext db = new SplendorDbContext();

        // GET: PlayerInGames
        public ActionResult Index()
        {
            var playerInGames = db.PlayerInGames.Include(p => p.Game).Include(p => p.Player);
            return View(playerInGames.ToList());
        }

        // GET: PlayerInGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerInGame playerInGame = db.PlayerInGames.Find(id);
            if (playerInGame == null)
            {
                return HttpNotFound();
            }
            return View(playerInGame);
        }

        // GET: PlayerInGames/Create
        public ActionResult Create()
        {
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name");
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "UserName");
            return View();
        }

        // POST: PlayerInGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ResourceGreen,ResourceWhite,ResourceBlue,ResourceBlack,ResourceRed,ResourceGold,IsWiner,GameId,PlayerId")] PlayerInGame playerInGame)
        {
            if (ModelState.IsValid)
            {
                db.PlayerInGames.Add(playerInGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", playerInGame.GameId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "UserName", playerInGame.PlayerId);
            return View(playerInGame);
        }

        // GET: PlayerInGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerInGame playerInGame = db.PlayerInGames.Find(id);
            if (playerInGame == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", playerInGame.GameId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "UserName", playerInGame.PlayerId);
            return View(playerInGame);
        }

        // POST: PlayerInGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ResourceGreen,ResourceWhite,ResourceBlue,ResourceBlack,ResourceRed,ResourceGold,IsWiner,GameId,PlayerId")] PlayerInGame playerInGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerInGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", playerInGame.GameId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "UserName", playerInGame.PlayerId);
            return View(playerInGame);
        }

        // GET: PlayerInGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerInGame playerInGame = db.PlayerInGames.Find(id);
            if (playerInGame == null)
            {
                return HttpNotFound();
            }
            return View(playerInGame);
        }

        // POST: PlayerInGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlayerInGame playerInGame = db.PlayerInGames.Find(id);
            db.PlayerInGames.Remove(playerInGame);
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
