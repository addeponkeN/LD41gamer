using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    class Map
    {

        public Point Size;
        public Point Position;

        public List<Bullet> Bullets;
        //public List<Enemy> Enemies;

        public Map()
        {
            Size = new Point(Globals.ScreenWidth, 100);
            Position = new Point(0, Globals.ScreenHeight - Size.Y);
        }


        public void Update(GameTime gt, GameScreen gs)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(UtilityContent.box, new Rectangle(Position, Size), Color.ForestGreen);
        }

    }
}
