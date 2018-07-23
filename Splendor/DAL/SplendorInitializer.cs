using Splendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splendor.DAL
{
    public class SplendorInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SplendorDbContext>
    {
        protected override void Seed(SplendorDbContext context)
        {
            var gems = new List<Gem>
            {
                new Gem() { Name = "green", ImagePath = "~/Images/Gem/gem0.png" },
                new Gem() { Name = "white", ImagePath = "~/Images/Gem/gem1.png" },
                new Gem() { Name = "blue", ImagePath = "~/Images/Gem/gem2.png" },
                new Gem() { Name = "black", ImagePath = "~/Images/Gem/gem3.png" },
                new Gem() { Name = "red", ImagePath = "~/Images/Gem/gem4.png" },
                new Gem() { Name = "gold", ImagePath = "~/Images/Gem/gem5.png" }
            };
            gems.ForEach(g => context.Gems.Add(g));
            context.SaveChanges();

            var aristocrats = new List<Aristocrat>
            {
                new Aristocrat() { Points = 3, RequireBlack = 3, RequireRed = 3,
                    RequireWhite = 3, ImagePath = "~/Images/Baron/baron0.png" },
                new Aristocrat() { Points = 3, RequireBlack = 3, RequireBlue = 3,
                    RequireWhite = 3, ImagePath = "~/Images/Baron/baron1.png" },
                new Aristocrat() { Points = 3, RequireBlack = 3, RequireRed = 3,
                    RequireGreen = 3, ImagePath = "~/Images/Baron/baron2.png" },
                new Aristocrat() { Points = 3, RequireGreen = 3, RequireBlue = 3,
                    RequireWhite = 3, ImagePath = "~/Images/Baron/baron3.png" },
                new Aristocrat() { Points = 3, RequireGreen = 3, RequireBlue = 3,
                    RequireRed = 3, ImagePath = "~/Images/Baron/baron4.png" },
                new Aristocrat() { Points = 3, RequireBlue = 4, RequireGreen = 4,
                    ImagePath = "~/Images/Baron/baron5.png" },
                new Aristocrat() { Points = 3, RequireBlack = 4, RequireWhite = 4,
                    ImagePath = "~/Images/Baron/baron6.png" },
                new Aristocrat() { Points = 3, RequireBlack = 4, RequireRed = 4,
                    ImagePath = "~/Images/Baron/baron7.png" },
                new Aristocrat() { Points = 3, RequireBlue = 4, RequireWhite = 4,
                    ImagePath = "~/Images/Baron/baron8.png" },
                new Aristocrat() { Points = 3, RequireRed = 4, RequireGreen = 4,
                    ImagePath = "~/Images/Baron/baron9.png" }
            };
            aristocrats.ForEach(a => context.Aristocrats.Add(a));
            context.SaveChanges();

            var card = new List<Card>
            {
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostWhite = 2, CostBlue = 1, ImagePath = "~/Images/Card/card00.png" },
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostRed = 3, ImagePath = "~/Images/Card/card01.png" },
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostBlue = 2, CostRed = 2, ImagePath = "~/Images/Card/card02.png" },
                new Card() { Points = 1, GemId = Gem.green, Lvl = 1, CostBlack = 4, ImagePath = "~/Images/Card/card03.png" },
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostWhite = 1, CostBlue = 1, CostRed = 1, CostBlack = 1, ImagePath = "~/Images/Card/card04.png" },
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostWhite = 1, CostBlue = 1, CostRed = 1, CostBlack = 2, ImagePath = "~/Images/Card/card05.png" },
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostWhite = 1, CostBlue = 3, CostGreen = 1, ImagePath = "~/Images/Card/card06.png" },
                new Card() { Points = 0, GemId = Gem.green, Lvl = 1, CostBlue = 1, CostRed = 2, CostBlack = 2, ImagePath = "~/Images/Card/card07.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostBlue = 3, ImagePath = "~/Images/Card/card08.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostRed = 2, CostBlack = 1, ImagePath = "~/Images/Card/card09.png" },
                new Card() { Points = 1, GemId = Gem.white, Lvl = 1, CostGreen = 4, ImagePath = "~/Images/Card/card10.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostBlue = 1, CostGreen = 1, CostRed = 1, CostBlack = 1, ImagePath = "~/Images/Card/card11.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostBlue = 2, CostBlack = 2, ImagePath = "~/Images/Card/card12.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostBlue = 2, CostGreen = 2, CostBlack = 1, ImagePath = "~/Images/Card/card13.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostWhite = 3, CostBlue = 1, CostBlack = 1, ImagePath = "~/Images/Card/card14.png" },
                new Card() { Points = 0, GemId = Gem.white, Lvl = 1, CostBlue = 1, CostGreen = 2, CostRed = 1, CostBlack = 1, ImagePath = "~/Images/Card/card15.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostBlue = 1, CostGreen = 3, CostRed = 1, ImagePath = "~/Images/Card/card16.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostWhite = 1, CostGreen = 2, CostRed = 2, ImagePath = "~/Images/Card/card17.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostWhite = 1, CostGreen = 1, CostRed = 2, CostBlack = 1, ImagePath = "~/Images/Card/card18.png" },
                new Card() { Points = 1, GemId = Gem.blue, Lvl = 1, CostRed = 4, ImagePath = "~/Images/Card/card19.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostWhite = 1, CostGreen = 1, CostRed = 1, CostBlack = 1, ImagePath = "~/Images/Card/card20.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostGreen = 2, CostBlack = 2, ImagePath = "~/Images/Card/card21.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostWhite = 1, CostBlack = 2, ImagePath = "~/Images/Card/card22.png" },
                new Card() { Points = 0, GemId = Gem.blue, Lvl = 1, CostBlack = 3, ImagePath = "~/Images/Card/card23.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostWhite = 1, CostBlue = 2, CostGreen = 1, CostRed = 1, ImagePath = "~/Images/Card/card24.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostGreen = 1, CostRed = 3, CostBlack = 1, ImagePath = "~/Images/Card/card25.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostGreen = 2, CostRed = 1, ImagePath = "~/Images/Card/card26.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostWhite = 2, CostGreen = 2, ImagePath = "~/Images/Card/card27.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostGreen = 3, ImagePath = "~/Images/Card/card28.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostWhite = 1, CostBlue = 1, CostGreen = 1, CostRed = 1, ImagePath = "~/Images/Card/card29.png" },
                new Card() { Points = 0, GemId = Gem.black, Lvl = 1, CostWhite = 2, CostBlue = 2, CostRed = 1, ImagePath = "~/Images/Card/card30.png" },
                new Card() { Points = 1, GemId = Gem.black, Lvl = 1, CostBlue = 4, ImagePath = "~/Images/Card/card31.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostWhite = 1, CostRed = 1, CostBlack = 3, ImagePath = "~/Images/Card/card32.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostWhite = 1, CostBlue = 1, CostGreen = 1, CostBlack = 1, ImagePath = "~/Images/Card/card33.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostBlue = 2, CostGreen = 1, ImagePath = "~/Images/Card/card34.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostWhite = 3, ImagePath = "~/Images/Card/card35.png" },
                new Card() { Points = 1, GemId = Gem.red, Lvl = 1, CostWhite = 4, ImagePath = "~/Images/Card/card36.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostWhite = 2, CostRed = 2, ImagePath = "~/Images/Card/card37.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostWhite = 2, CostGreen = 1, CostBlack = 2, ImagePath = "~/Images/Card/card38.png" },
                new Card() { Points = 0, GemId = Gem.red, Lvl = 1, CostWhite = 2, CostBlue = 1, CostGreen = 1, CostBlack = 1, ImagePath = "~/Images/Card/card39.png" },
                new Card() { Points = 1, GemId = Gem.green, Lvl = 2, CostWhite = 3, CostGreen = 2, CostRed = 3, ImagePath = "~/Images/Card/card40.png" },
                new Card() { Points = 1, GemId = Gem.green, Lvl = 2, CostWhite = 2, CostBlue = 3, CostBlack = 2, ImagePath = "~/Images/Card/card41.png" },
                new Card() { Points = 2, GemId = Gem.green, Lvl = 2, CostWhite = 4, CostBlue = 2, CostBlack = 1, ImagePath = "~/Images/Card/card42.png" },
                new Card() { Points = 2, GemId = Gem.green, Lvl = 2, CostGreen = 5, ImagePath = "~/Images/Card/card43.png" },
                new Card() { Points = 2, GemId = Gem.green, Lvl = 2, CostBlue = 5, CostGreen = 3, ImagePath = "~/Images/Card/card44.png" },
                new Card() { Points = 3, GemId = Gem.green, Lvl = 2, CostGreen = 6, ImagePath = "~/Images/Card/card45.png" },
                new Card() { Points = 1, GemId = Gem.white, Lvl = 2, CostWhite = 2, CostBlue = 3, CostRed = 3, ImagePath = "~/Images/Card/card46.png" },
                new Card() { Points = 1, GemId = Gem.white, Lvl = 2, CostGreen = 3, CostRed = 2, CostBlack = 2, ImagePath = "~/Images/Card/card47.png" },
                new Card() { Points = 2, GemId = Gem.white, Lvl = 2, CostGreen = 1, CostRed = 4, CostBlack = 2, ImagePath = "~/Images/Card/card48.png" },
                new Card() { Points = 2, GemId = Gem.white, Lvl = 2, CostRed = 5, CostBlack = 3, ImagePath = "~/Images/Card/card49.png" },
                new Card() { Points = 2, GemId = Gem.white, Lvl = 2, CostRed = 5, ImagePath = "~/Images/Card/card50.png" },
                new Card() { Points = 3, GemId = Gem.white, Lvl = 2, CostWhite = 6, ImagePath = "~/Images/Card/card51.png" },
                new Card() { Points = 1, GemId = Gem.blue, Lvl = 2, CostBlue = 2, CostGreen = 2, CostRed = 3, ImagePath = "~/Images/Card/card52.png" },
                new Card() { Points = 1, GemId = Gem.blue, Lvl = 2, CostBlue = 2, CostGreen = 3, CostBlack = 3, ImagePath = "~/Images/Card/card53.png" },
                new Card() { Points = 2, GemId = Gem.blue, Lvl = 2, CostWhite = 5, CostBlue = 3, ImagePath = "~/Images/Card/card54.png" },
                new Card() { Points = 2, GemId = Gem.blue, Lvl = 2, CostBlue = 5, ImagePath = "~/Images/Card/card55.png" },
                new Card() { Points = 2, GemId = Gem.blue, Lvl = 2, CostWhite = 2, CostRed = 1, CostBlack = 4, ImagePath = "~/Images/Card/card56.png" },
                new Card() { Points = 3, GemId = Gem.blue, Lvl = 2, CostBlue = 6, ImagePath = "~/Images/Card/card57.png" },
                new Card() { Points = 1, GemId = Gem.black, Lvl = 2, CostWhite = 3, CostBlue = 2, CostGreen = 2, ImagePath = "~/Images/Card/card58.png" },
                new Card() { Points = 1, GemId = Gem.black, Lvl = 2, CostWhite = 3, CostGreen = 3, CostBlack = 2, ImagePath = "~/Images/Card/card59.png" },
                new Card() { Points = 2, GemId = Gem.black, Lvl = 2, CostBlue = 1, CostGreen = 4, CostRed = 2, ImagePath = "~/Images/Card/card60.png" },
                new Card() { Points = 2, GemId = Gem.black, Lvl = 2, CostWhite = 5, ImagePath = "~/Images/Card/card61.png" },
                new Card() { Points = 2, GemId = Gem.black, Lvl = 2, CostGreen = 5, CostRed = 3, ImagePath = "~/Images/Card/card62.png" },
                new Card() { Points = 3, GemId = Gem.black, Lvl = 2, CostBlack = 6, ImagePath = "~/Images/Card/card63.png" },
                new Card() { Points = 1, GemId = Gem.red, Lvl = 2, CostWhite = 2, CostRed = 2, CostBlack = 3, ImagePath = "~/Images/Card/card64.png" },
                new Card() { Points = 1, GemId = Gem.red, Lvl = 2, CostBlue = 3, CostRed = 2, CostBlack = 3, ImagePath = "~/Images/Card/card65.png" },
                new Card() { Points = 2, GemId = Gem.red, Lvl = 2, CostWhite = 1, CostBlue = 4, CostGreen = 2, ImagePath = "~/Images/Card/card66.png" },
                new Card() { Points = 2, GemId = Gem.red, Lvl = 2, CostBlack = 5, ImagePath = "~/Images/Card/card67.png" },
                new Card() { Points = 2, GemId = Gem.red, Lvl = 2, CostWhite = 3, CostBlack = 5, ImagePath = "~/Images/Card/card68.png" },
                new Card() { Points = 3, GemId = Gem.red, Lvl = 2, CostRed = 6, ImagePath = "~/Images/Card/card69.png" },
                new Card() { Points = 3, GemId = Gem.green, Lvl = 3, CostWhite = 5, CostBlue = 3, CostRed = 3, CostBlack = 3, ImagePath = "~/Images/Card/card70.png" },
                new Card() { Points = 4, GemId = Gem.green, Lvl = 3, CostBlue = 7, ImagePath = "~/Images/Card/card71.png" },
                new Card() { Points = 4, GemId = Gem.green, Lvl = 3, CostWhite = 3, CostBlue = 6, CostGreen = 3, ImagePath = "~/Images/Card/card72.png" },
                new Card() { Points = 5, GemId = Gem.green, Lvl = 3, CostBlue = 7, CostGreen = 3, ImagePath = "~/Images/Card/card73.png" },
                new Card() { Points = 3, GemId = Gem.white, Lvl = 3, CostBlue = 3, CostGreen = 3, CostRed = 5, CostBlack = 3, ImagePath = "~/Images/Card/card74.png" },
                new Card() { Points = 4, GemId = Gem.white, Lvl = 3, CostBlack = 7, ImagePath = "~/Images/Card/card75.png" },
                new Card() { Points = 4, GemId = Gem.white, Lvl = 3, CostWhite = 3, CostRed = 3, CostBlack = 6, ImagePath = "~/Images/Card/card76.png" },
                new Card() { Points = 5, GemId = Gem.white, Lvl = 3, CostWhite = 3, CostBlack = 7, ImagePath = "~/Images/Card/card77.png" },
                new Card() { Points = 3, GemId = Gem.blue, Lvl = 3, CostWhite = 3, CostGreen = 3, CostRed = 3, CostBlack = 5, ImagePath = "~/Images/Card/card78.png" },
                new Card() { Points = 4, GemId = Gem.blue, Lvl = 3, CostWhite = 7, ImagePath = "~/Images/Card/card79.png" },
                new Card() { Points = 4, GemId = Gem.blue, Lvl = 3, CostWhite = 6, CostBlue = 3, CostBlack = 3, ImagePath = "~/Images/Card/card80.png" },
                new Card() { Points = 5, GemId = Gem.blue, Lvl = 3, CostWhite = 7, CostBlue = 3, ImagePath = "~/Images/Card/card81.png" },
                new Card() { Points = 3, GemId = Gem.black, Lvl = 3, CostWhite = 3, CostBlue = 3, CostGreen = 5, CostRed = 3, ImagePath = "~/Images/Card/card82.png" },
                new Card() { Points = 4, GemId = Gem.black, Lvl = 3, CostRed = 7, ImagePath = "~/Images/Card/card83.png" },
                new Card() { Points = 4, GemId = Gem.black, Lvl = 3, CostGreen = 3, CostRed = 6, CostBlack = 3, ImagePath = "~/Images/Card/card84.png" },
                new Card() { Points = 5, GemId = Gem.black, Lvl = 3, CostRed = 7, CostBlack = 3, ImagePath = "~/Images/Card/card85.png" },
                new Card() { Points = 3, GemId = Gem.red, Lvl = 3, CostWhite = 3, CostBlue = 5, CostGreen = 3, CostBlack = 3, ImagePath = "~/Images/Card/card86.png" },
                new Card() { Points = 4, GemId = Gem.red, Lvl = 3, CostGreen = 7, ImagePath = "~/Images/Card/card87.png" },
                new Card() { Points = 4, GemId = Gem.red, Lvl = 3, CostBlue = 3, CostGreen = 6, CostRed = 3, ImagePath = "~/Images/Card/card88.png" },
                new Card() { Points = 5, GemId = Gem.red, Lvl = 3, CostGreen = 7, CostRed = 3, ImagePath = "~/Images/Card/card89.png" }
            };

            card.ForEach(c => context.Cards.Add(c));
            context.SaveChanges();

            context.Players.Add(new Player() { UserName = "Table" });
            context.Players.Add(new Player() { UserName = "Observer" });
            context.SaveChanges();
        }
    }
}