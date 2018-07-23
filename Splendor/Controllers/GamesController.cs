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
using Splendor.Helpers;
using PagedList;

namespace Splendor.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        #region Fields

        private static object CreationLock = new object();

        private static object JoinLock = new object();

        private SplendorDbContext db = new SplendorDbContext();

        #endregion

        #region Actions

        [AllowAnonymous]
        // GET: Games
        public ActionResult Index(string searchString, int? pagePlayerStartedNumber, int? pagePlayerCreatedNumber, int? pageCreatedNumber, int? pageStartedNumber, int? pageEndedNumber)
        {
            ViewBag.SearchString = searchString;

            IEnumerable<Game> games = db.Games.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(x => x.Name.ToLower().Replace(' ', '_').Contains(searchString.ToLower().Replace(' ', '_')));
            }

            IOrderedEnumerable<Game> orderedGames = games.OrderByDescending(x => x.CreationTime);

            ViewBag.PageSize = 5;
            ViewBag.PagePlayerStartedNumber = (pagePlayerStartedNumber ?? 1);
            ViewBag.PagePlayerCreatedNumber = (pagePlayerCreatedNumber ?? 1);
            ViewBag.PageCreatedNumber = (pageCreatedNumber ?? 1);
            ViewBag.PageStartedNumber = (pageStartedNumber ?? 1);
            ViewBag.PageEndedNumber = (pageEndedNumber ?? 1);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexTables", orderedGames);
            }
            else
            {
                return View(orderedGames);
            }
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.Gems = db.Gems.ToList();
            if (game.IsEnded)
            {
                ViewBag.EndGameInfo = true;
            }
            if (Request.IsAjaxRequest())
            {
                if (game.IsStarted)
                {
                    return PartialView("_PlayGame", game);
                }
                else
                {
                    return PartialView("_DetailsPartial", game);
                }
            }
            else
            {
                return View(game);
            }
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Game game)
        {
            lock (CreationLock)
            {
                if (db.Games.ToList().Any(x => x.Name.Equals(game.Name) && x.IsEnded == false))
                {
                    // TODO Odpowiedź dla użytkownika.
                    db.Logs.Add(new Log(User.Identity.Name, Log.Warning, null, String.Format("Game with name {0} created at {1} already exists and is being stil played.", game.Name, game.CreationTime)));
                    db.SaveChanges();
                    return View(game);
                }

                if (ModelState.IsValid)
                {
                    db.Games.Add(game);
                    db.SaveChanges();
                    AddCurrentPlayer(game.Id);
                    db.Logs.Add(new Log(User.Identity.Name, Log.System, game.Id, String.Format("Game {0} has been created.", game.Name)));
                    db.SaveChanges();
                    return RedirectToAction("Details", "Games", new { Id = game.Id });
                }

                db.Logs.Add(new Log(User.Identity.Name, Log.Warning, null, String.Format("Game {0} has not been created.", game.Name)));
                db.SaveChanges();
            }
            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize(Users = UserRoles.UserAdmin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Users = UserRoles.UserAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize(Users = UserRoles.UserAdmin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [Authorize(Users = UserRoles.UserAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Games/Join/5
        public ActionResult Join(int id)
        {
            lock (JoinLock)
            {
                Game game = db.Games.Find(id);
                if (game == null)
                {
                    return HttpNotFound();
                }
                AddCurrentPlayer(id);
            }
            return RedirectToAction("Details", new { id = id });
        }

        // Games/Play/5
        public ActionResult Play(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            if (game.IsStarted)
            {
                db.Logs.Add(new Log(User.Identity.Name, Log.Error, id, String.Format("Game {0} is already started and cannot be started second time!", game.Name)));
                RedirectToAction("Details", new { id = id });
            }

            int resource = 0;
            switch (game.PlayerInGames.Count)
            {
                case 2: resource = 4; break;
                case 3: resource = 5; break;
                case 4: resource = 7; break;
                default: resource = 0; break;
            }

            IList<PlayerInGame> playerInThisGame = new List<PlayerInGame>(db.PlayerInGames.Where(x => x.GameId.Equals(id)).ToList());
            playerInThisGame.Shuffle();
            playerInThisGame.FirstOrDefault().IsActive = true;
            game.FirstPlayerInGameId = playerInThisGame.FirstOrDefault().Id;
            for (int i = 0; i < playerInThisGame.Count; ++i)
            {
                playerInThisGame.ElementAt(i).NextPlayerInGameId = playerInThisGame.ElementAt((i + 1) % playerInThisGame.Count).Id;
            }
            db.SaveChanges();

            db.PlayerInGames.Add(new PlayerInGame()
            {
                GameId = id,
                PlayerId = 1,
                ResourceGreen = resource,
                ResourceWhite = resource,
                ResourceBlue = resource,
                ResourceBlack = resource,
                ResourceRed = resource,
                ResourceGold = 5
            });

            db.PlayerInGames.Add(new PlayerInGame()
            {
                GameId = id,
                PlayerId = 2,
                ResourceGreen = 0,
                ResourceWhite = 0,
                ResourceBlue = 0,
                ResourceBlack = 0,
                ResourceRed = 0,
                ResourceGold = 0
            });
            db.SaveChanges();

            int tableInGameId = game.TableInGame.Id;
            foreach (var item in db.Aristocrats.ToList().TakeRandom(game.PlayerInGames.Count - 1))
            {
                db.AristocratInGames.Add(new AristocratInGame()
                {
                    AristocratId = item.Id,
                    PlayerInGameId = tableInGameId
                });
            }
            db.SaveChanges();

            foreach (var item in db.Cards)
            {
                db.CardInGames.Add(new CardInGame()
                {
                    CardId = item.Id,
                    PlayerInGameId = tableInGameId
                });
            }
            db.SaveChanges();

            foreach (var item in db.CardInGames.Where(x => x.PlayerInGameId.Equals(tableInGameId) && x.Card.Lvl.Equals(1)).ToList().TakeRandom(4))
            {
                item.IsOnTable = true;
            }
            foreach (var item in db.CardInGames.Where(x => x.PlayerInGameId.Equals(tableInGameId) && x.Card.Lvl.Equals(2)).ToList().TakeRandom(4))
            {
                item.IsOnTable = true;
            }
            foreach (var item in db.CardInGames.Where(x => x.PlayerInGameId.Equals(tableInGameId) && x.Card.Lvl.Equals(3)).ToList().TakeRandom(4))
            {
                item.IsOnTable = true;
            }
            db.SaveChanges();

            db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Game {0} has been started.", game.Name)));
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult TakeRandomCard(int id, int playerInGameId, int lvl)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);
            CardInGame cardInGame;

            try
            {
                cardInGame = db.CardInGames.Where(x => x.PlayerInGame.GameId.Equals(id) && x.IsOnTable == false && x.Card.Lvl.Equals(lvl)).ToList().TakeRandom(1).FirstOrDefault();
            }
            catch (ArgumentOutOfRangeException)
            {
                db.Logs.Add(new Log(User.Identity.Name, Log.Warning, id, String.Format("The card pile is empty when user wants to take a card from it.")));
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }

            if (playerInGame.CardInGames.Where(x => x.IsOnTable == false).Count() >= 3)
            {
                db.Logs.Add(new Log(User.Identity.Name, Log.Warning, id, String.Format("Player {0} cannot reserve more than 3 cards.", playerInGame.Name)));
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            ReserveCard(cardInGame, playerInGame);
            SetNextPlayer(playerInGameId);

            db.Logs.Add(new Log(User.Identity.Name, Log.GameLog, id, String.Format("User {0} has reserved card {1} in game {2}.", playerInGame.Name, cardInGame.CardId, game.Name)));
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult TakeCard(int id, int cardInGameId, int playerInGameId, bool reserve = false)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            CardInGame cardInGame = db.CardInGames.FirstOrDefault(x => x.Id.Equals(cardInGameId));
            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);
            PlayerInGame tableInGame = game.TableInGame;

            if (reserve)
            {
                if (playerInGame.CardInGames.Where(x => x.IsOnTable == false).Count() >= 3)
                {
                    db.Logs.Add(new Log(User.Identity.Name, Log.Warning, id, String.Format("Player {0} cannot reserve more than 3 cards.", playerInGame.Name)));
                    return RedirectToAction("Details", new { id = id });
                }
                ReserveCard(cardInGame, playerInGame);
                AddNewCard(id, cardInGame.Card.Lvl);
                SetNextPlayer(playerInGameId);
                db.Logs.Add(new Log(User.Identity.Name, Log.GameLog, id, String.Format("Player {0} has reserved card {1} in game {2}.", playerInGame.Name, cardInGame.CardId, game.Name)));
                db.SaveChanges();
            }
            else
            {
                int costGold;
                if (playerInGame.CanAfford(cardInGame, out costGold))
                {
                    int transferGold = 0;
                    transferGold += TransferGem(playerInGame, tableInGame, cardInGame.Card.CostGreen - playerInGame.CardInGames.Where(x => x.Card.GemId == Gem.green && x.IsOnTable == true).Count(), Gem.green);
                    transferGold += TransferGem(playerInGame, tableInGame, cardInGame.Card.CostWhite - playerInGame.CardInGames.Where(x => x.Card.GemId == Gem.white && x.IsOnTable == true).Count(), Gem.white);
                    transferGold += TransferGem(playerInGame, tableInGame, cardInGame.Card.CostBlack - playerInGame.CardInGames.Where(x => x.Card.GemId == Gem.black && x.IsOnTable == true).Count(), Gem.black);
                    transferGold += TransferGem(playerInGame, tableInGame, cardInGame.Card.CostBlue - playerInGame.CardInGames.Where(x => x.Card.GemId == Gem.blue && x.IsOnTable == true).Count(), Gem.blue);
                    transferGold += TransferGem(playerInGame, tableInGame, cardInGame.Card.CostRed - playerInGame.CardInGames.Where(x => x.Card.GemId == Gem.red && x.IsOnTable == true).Count(), Gem.red);
                    int lackOfGems = TransferGem(playerInGame, tableInGame, transferGold, Gem.gold);
                    if (lackOfGems > 0)
                    {
                        db.Logs.Add(new Log(User.Identity.Name, Log.Error, id, String.Format("An critical error occured while transferring gems. Player {0} try to buy card {1} using {2} golden gems in game {3}.", playerInGame.Name, cardInGame.CardId, costGold, game.Name)));
                        db.SaveChanges();
                        throw new InvalidOperationException("An critical error occured while transferring gems! Please contact with administrator.");
                    }
                    if (cardInGame.IsOnTable == true && tableInGame.CardInGames.Any(x => x.IsOnTable == false && x.Card.Lvl.Equals(cardInGame.Card.Lvl)))
                    {
                        AddNewCard(id, cardInGame.Card.Lvl);
                    }
                    cardInGame.PlayerInGameId = playerInGameId;
                    cardInGame.IsOnTable = true;
                    db.SaveChanges();
                    SetNextPlayer(playerInGameId);
                    db.Logs.Add(new Log(User.Identity.Name, Log.GameLog, id, String.Format("User {0} has bought card {1} using {2} golden gems in game {3}.", playerInGame.Name, cardInGame.CardId, costGold, game.Name)));
                    db.SaveChanges();
                }
                else
                {
                    db.Logs.Add(new Log(User.Identity.Name, Log.Warning, id, String.Format("User {0} has not enough gems to buy card {1}. {2}/{3} green, {4}/{5} white, {6}/{7} blue, {8}/{9} black, {10}/{11} red, {12}/{13} gold in game {14}.",
                        playerInGame.Name, cardInGame.CardId,
                        playerInGame.ResourceGreen, cardInGame.Card.CostGreen,
                        playerInGame.ResourceWhite, cardInGame.Card.CostWhite,
                        playerInGame.ResourceBlue, cardInGame.Card.CostBlue,
                        playerInGame.ResourceBlack, cardInGame.Card.CostBlack,
                        playerInGame.ResourceRed, cardInGame.Card.CostRed,
                        playerInGame.ResourceGold, costGold, game.Name)));
                    db.SaveChanges();
                    // TODO Redirect to page: You have not enough gems to buy this card
                }
            }
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult ChangeReserving(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            PlayerInGame currentPlayer = GetCurrentPlayerInGame(id);
            currentPlayer.IsReserving = !currentPlayer.IsReserving;
            db.SaveChanges();
            db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} wants to {1} cards in game {2}.", currentPlayer.Name, currentPlayer.IsReserving ? "reserve" : "buy", game.Name)));
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult TakeGem(int id, int gemId, int playerInGameId)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);
            PlayerInGame tableInGame = game.TableInGame;

            if (playerInGame.CanCollect(gemId))
            {
                switch (gemId)
                {
                    case Gem.green:
                        ++playerInGame.TakeGreen;
                        --tableInGame.ResourceGreen;
                        break;
                    case Gem.white:
                        ++playerInGame.TakeWhite;
                        --tableInGame.ResourceWhite;
                        break;
                    case Gem.blue:
                        ++playerInGame.TakeBlue;
                        --tableInGame.ResourceBlue;
                        break;
                    case Gem.black:
                        ++playerInGame.TakeBlack;
                        --tableInGame.ResourceBlack;
                        break;
                    case Gem.red:
                        ++playerInGame.TakeRed;
                        --tableInGame.ResourceRed;
                        break;
                }
                db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} has collected gem {1} in game {2}.", playerInGame.Name, gemId, game.Name)));
            }
            else
            {
                db.Logs.Add(new Log(User.Identity.Name, Log.Warning, id, String.Format("Player {0} cannot take more gem {1} in game.", playerInGame.Name, gemId, game.Name)));
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult GiveGem(int id, int gemId, int playerInGameId)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);
            PlayerInGame tableInGame = game.TableInGame;

            switch (gemId)
            {
                case Gem.green:
                    if (playerInGame.TakeGreen > 0)
                    {
                        --playerInGame.TakeGreen;
                        ++tableInGame.ResourceGreen;
                        db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} has given back gem {1} in game {2}.", playerInGame.Name, gemId, game.Name)));
                    }
                    break;
                case Gem.white:
                    if (playerInGame.TakeWhite > 0)
                    {
                        --playerInGame.TakeWhite;
                        ++tableInGame.ResourceWhite;
                        db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} has given back gem {1} in game {2}.", playerInGame.Name, gemId, game.Name)));
                    }
                    break;
                case Gem.blue:
                    if (playerInGame.TakeBlue > 0)
                    {
                        --playerInGame.TakeBlue;
                        ++tableInGame.ResourceBlue;
                        db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} has given back gem {1} in game {2}.", playerInGame.Name, gemId, game.Name)));
                    }
                    break;
                case Gem.black:
                    if (playerInGame.TakeBlack > 0)
                    {
                        --playerInGame.TakeBlack;
                        ++tableInGame.ResourceBlack;
                        db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} has given back gem {1} in game {2}.", playerInGame.Name, gemId, game.Name)));
                    }
                    break;
                case Gem.red:
                    if (playerInGame.TakeRed > 0)
                    {
                        --playerInGame.TakeRed;
                        ++tableInGame.ResourceRed;
                        db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player {0} has given back gem {1} in game {2}.", playerInGame.Name, gemId, game.Name)));
                    }
                    break;
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult TakeCollectedGems(int id, int playerInGameId)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);

            db.Logs.Add(new Log(User.Identity.Name, Log.GameLog, id, String.Format("Player {0} has taken {1} green, {2} white, {3} blue, {4} black, {5} red gems in game {6}.", playerInGame.Name, playerInGame.TakeGreen, playerInGame.TakeWhite, playerInGame.TakeBlue, playerInGame.TakeBlack, playerInGame.TakeRed, game.Name)));
            playerInGame.ResourceGreen += playerInGame.TakeGreen;
            playerInGame.TakeGreen = 0;
            playerInGame.ResourceWhite += playerInGame.TakeWhite;
            playerInGame.TakeWhite = 0;
            playerInGame.ResourceBlue += playerInGame.TakeBlue;
            playerInGame.TakeBlue = 0;
            playerInGame.ResourceBlack += playerInGame.TakeBlack;
            playerInGame.TakeBlack = 0;
            playerInGame.ResourceRed += playerInGame.TakeRed;
            playerInGame.TakeRed = 0;
            db.SaveChanges();
            SetNextPlayer(playerInGameId);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult CancelCollectingGems(int id, int playerInGameId)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);
            PlayerInGame tableInGame = game.TableInGame;

            db.Logs.Add(new Log(User.Identity.Name, Log.System, id, String.Format("Player cancel collecting gems in game {0}.", game.Name)));
            tableInGame.ResourceGreen += playerInGame.TakeGreen;
            playerInGame.TakeGreen = 0;
            tableInGame.ResourceWhite += playerInGame.TakeWhite;
            playerInGame.TakeWhite = 0;
            tableInGame.ResourceBlue += playerInGame.TakeBlue;
            playerInGame.TakeBlue = 0;
            tableInGame.ResourceBlack += playerInGame.TakeBlack;
            playerInGame.TakeBlack = 0;
            tableInGame.ResourceRed += playerInGame.TakeRed;
            playerInGame.TakeRed = 0;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private void AddCurrentPlayer(int gameId)
        {
            Player currentPlayer = db.Players.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name));
            if (!db.PlayerInGames.Any(x => x.PlayerId.Equals(currentPlayer.Id) && x.GameId.Equals(gameId)))
            {
                db.PlayerInGames.Add(new PlayerInGame() { GameId = gameId, PlayerId = currentPlayer.Id });
                db.SaveChanges();
            }
            db.Logs.Add(new Log(User.Identity.Name, Log.System, gameId, String.Format("Player {0} has join to game {1}.", db.PlayerInGames.FirstOrDefault(x => x.GameId.Equals(gameId) && x.PlayerId.Equals(currentPlayer.Id)).Name, db.Games.Find(gameId).Name)));
            db.SaveChanges();
        }

        private void SetNextPlayer(int playerInGameId)
        {
            PlayerInGame playerInGame = db.PlayerInGames.Find(playerInGameId);
            TakeAristocrat(playerInGame);
            playerInGame.IsReserving = false;
            playerInGame.IsActive = false;
            PlayerInGame nextPlayerInGame = db.PlayerInGames.Find(playerInGame.NextPlayerInGameId);
            if (!IsEndGame(nextPlayerInGame))
            {
                nextPlayerInGame.IsActive = true;
                db.Logs.Add(new Log(User.Identity.Name, Log.System, playerInGame.GameId, String.Format("Player {0} end his turn and now {1} is active player in game {2}.", playerInGame.Name, nextPlayerInGame.Name, playerInGame.Game.Name)));
            }
            else
            {
                ViewBag.EndGameInfo = true;
            }
            db.SaveChanges();
        }

        private bool IsEndGame(PlayerInGame playerInGame)
        {
            IEnumerable<PlayerInGame> playersInGame = playerInGame.Game.PlayerInGames.Where(x => x.Id.Equals(playerInGame.Game.TableInGame.Id) == false);
            PlayerInGame bestPlayerInGame = playersInGame.OrderByDescending(x => x.Points).FirstOrDefault();
            if (bestPlayerInGame.Points >= 15)
            {
                bestPlayerInGame = playersInGame.Where(x => x.Points.Equals(bestPlayerInGame.Points)).OrderBy(x => x.CardInGames.Count()).FirstOrDefault();
                bestPlayerInGame.IsWiner = true;
                db.Logs.Add(new Log(User.Identity.Name, Log.GameLog, playerInGame.GameId, String.Format("Game {0} is ended. The winner is {1}.", bestPlayerInGame.Game.Name, bestPlayerInGame.Name)));
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        private PlayerInGame GetCurrentPlayerInGame(int id)
        {
            return db.PlayerInGames.Where(x => x.GameId == id && x.Player.UserName.Equals(User.Identity.Name)).FirstOrDefault();
        }

        private void ReserveCard(CardInGame cardInGame, PlayerInGame playerInGame)
        {
            TransferGem(cardInGame.PlayerInGame, playerInGame, 1, Gem.gold);
            cardInGame.PlayerInGameId = playerInGame.Id;
            cardInGame.IsOnTable = false;
            db.SaveChanges();
        }

        private void AddNewCard(int gameId, int lvl)
        {
            CardInGame cardInGame = db.CardInGames.Where(x => x.PlayerInGame.GameId.Equals(gameId) && x.PlayerInGame.PlayerId.Equals(1) && x.IsOnTable == false && x.Card.Lvl.Equals(lvl)).ToList().TakeRandom(1).FirstOrDefault();
            cardInGame.IsOnTable = true;
            db.Logs.Add(new Log(User.Identity.Name, Log.System, gameId, String.Format("Card {0} has been placed on the table in game {1}.", cardInGame.CardId, cardInGame.PlayerInGame.Game.Name)));
            db.SaveChanges();
        }

        /// <summary>
        /// Function transfer @amount gems from @fromPlayerInGame to @toPlayerInGame.
        /// </summary>
        /// <param name="fromPlayerInGame"></param>
        /// <param name="toPlayerInGame"></param>
        /// <param name="amount"></param>
        /// <param name="gemId"></param>
        /// <returns>
        /// Returns number of gems which have not been tranfered, because @fromPlayerInGame has not enough gems.
        /// In other words. Returns the difference between @amount to transfer and transferred amount in fact.
        /// </returns>
        private int TransferGem(PlayerInGame fromPlayerInGame, PlayerInGame toPlayerInGame, int amount, int gemId)
        {
            if (amount <= 0)
                return 0;
            int transferAmount = 0;
            switch (gemId)
            {
                case Gem.green:
                    transferAmount = Math.Min(amount, fromPlayerInGame.ResourceGreen);
                    fromPlayerInGame.ResourceGreen -= transferAmount;
                    toPlayerInGame.ResourceGreen += transferAmount;
                    break;
                case Gem.white:
                    transferAmount = Math.Min(amount, fromPlayerInGame.ResourceWhite);
                    fromPlayerInGame.ResourceWhite -= transferAmount;
                    toPlayerInGame.ResourceWhite += transferAmount;
                    break;
                case Gem.blue:
                    transferAmount = Math.Min(amount, fromPlayerInGame.ResourceBlue);
                    fromPlayerInGame.ResourceBlue -= transferAmount;
                    toPlayerInGame.ResourceBlue += transferAmount;
                    break;
                case Gem.black:
                    transferAmount = Math.Min(amount, fromPlayerInGame.ResourceBlack);
                    fromPlayerInGame.ResourceBlack -= transferAmount;
                    toPlayerInGame.ResourceBlack += transferAmount;
                    break;
                case Gem.red:
                    transferAmount = Math.Min(amount, fromPlayerInGame.ResourceRed);
                    fromPlayerInGame.ResourceRed -= transferAmount;
                    toPlayerInGame.ResourceRed += transferAmount;
                    break;
                case Gem.gold:
                    transferAmount = Math.Min(amount, fromPlayerInGame.ResourceGold);
                    fromPlayerInGame.ResourceGold -= transferAmount;
                    toPlayerInGame.ResourceGold += transferAmount;
                    break;
            }
            db.Logs.Add(new Log(User.Identity.Name, Log.System, toPlayerInGame.GameId, String.Format("{0} gems {1} has been transferred from {2} to {3} in game {4}.", transferAmount, gemId, fromPlayerInGame.Name, toPlayerInGame.Name, fromPlayerInGame.Game.Name)));
            db.SaveChanges();
            return amount - transferAmount;
        }

        private void TakeAristocrat(PlayerInGame playerInGame)
        {
            AristocratInGame aristocratInGame = playerInGame.Game.TableInGame.AristocratInGames.FirstOrDefault
            (
                x =>
                playerInGame.CardInGames.Where(y => y.Card.GemId.Equals(Gem.green) && y.IsOnTable).Count() >= x.Aristocrat.RequireGreen &&
                playerInGame.CardInGames.Where(y => y.Card.GemId.Equals(Gem.white) && y.IsOnTable).Count() >= x.Aristocrat.RequireWhite &&
                playerInGame.CardInGames.Where(y => y.Card.GemId.Equals(Gem.blue) && y.IsOnTable).Count() >= x.Aristocrat.RequireBlue &&
                playerInGame.CardInGames.Where(y => y.Card.GemId.Equals(Gem.black) && y.IsOnTable).Count() >= x.Aristocrat.RequireBlack &&
                playerInGame.CardInGames.Where(y => y.Card.GemId.Equals(Gem.red) && y.IsOnTable).Count() >= x.Aristocrat.RequireRed
            );
            if (aristocratInGame != null)
            {
                aristocratInGame.PlayerInGameId = playerInGame.Id;
                db.Logs.Add(new Log(User.Identity.Name, Log.GameLog, playerInGame.GameId, String.Format("Player {0} take aristocrat {1} in game {2}.", playerInGame.Name, aristocratInGame.Aristocrat.Id, playerInGame.Game.Name)));
                db.SaveChanges();
            }
        }

        #endregion
    }
}
