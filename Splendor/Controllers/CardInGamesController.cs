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
    public class CardInGamesController : Controller
    {
        private SplendorDbContext db = new SplendorDbContext();

        // GET: CardInGames
        public ActionResult Index()
        {
            var cardInGames = db.CardInGames.Include(c => c.Card).Include(c => c.PlayerInGame);
            return View(cardInGames.ToList());
        }

        // GET: CardInGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardInGame cardInGame = db.CardInGames.Find(id);
            if (cardInGame == null)
            {
                return HttpNotFound();
            }
            return View(cardInGame);
        }

        // GET: CardInGames/Create
        public ActionResult Create()
        {
            ViewBag.CardId = new SelectList(db.Cards, "Id", "ImagePath");
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id");
            return View();
        }

        // POST: CardInGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CardId,PlayerInGameId")] CardInGame cardInGame)
        {
            if (ModelState.IsValid)
            {
                db.CardInGames.Add(cardInGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardId = new SelectList(db.Cards, "Id", "ImagePath", cardInGame.CardId);
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id", cardInGame.PlayerInGameId);
            return View(cardInGame);
        }

        // GET: CardInGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardInGame cardInGame = db.CardInGames.Find(id);
            if (cardInGame == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "ImagePath", cardInGame.CardId);
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id", cardInGame.PlayerInGameId);
            return View(cardInGame);
        }

        // POST: CardInGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CardId,PlayerInGameId")] CardInGame cardInGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardInGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "ImagePath", cardInGame.CardId);
            ViewBag.PlayerInGameId = new SelectList(db.PlayerInGames, "Id", "Id", cardInGame.PlayerInGameId);
            return View(cardInGame);
        }

        // GET: CardInGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardInGame cardInGame = db.CardInGames.Find(id);
            if (cardInGame == null)
            {
                return HttpNotFound();
            }
            return View(cardInGame);
        }

        // POST: CardInGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CardInGame cardInGame = db.CardInGames.Find(id);
            db.CardInGames.Remove(cardInGame);
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
