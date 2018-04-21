using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    class Map
    {

        public static float Gravity = 800f;

        public Point Size;
        public Point Position;

        public Rectangle Rectangle => new Rectangle(Position, Size);

        public List<Bullet> Bullets;
        //public List<Enemy> Enemies;

        public List<Sprite> Props = new List<Sprite>();

        public Map()
        {
            Size = new Point(Globals.ScreenWidth, 100);
            Position = new Point(0, Globals.ScreenHeight - Size.Y);

            for(int i = 0; i < 30; i++)
            {
                var p = new Sprite();
                p.SetSize(32);
                p.Color = Color.DarkSeaGreen;
                p.Position = new Vector2(5 * (i * 36), Rectangle.Top - Rng.Noxt(24, 40));
                Props.Add(p);
            }

        }

        public void Update(GameTime gt, GameScreen gs)
        {

        }

        public void CheckCollision(Player p)
        {
            p.Collision(Rectangle);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(UtilityContent.box, Rectangle, Color.ForestGreen);

            foreach(var p in Props)
            {
                p.Draw(sb);
            }
        }

    }
}
