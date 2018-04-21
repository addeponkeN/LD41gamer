using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
using ld41gamer.Gamer.StateMachine.GameStates;
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
    public class Map
    {

        public static float Gravity = 800f;

        public Point Size;
        public Point Position;

        public Rectangle Rectangle => new Rectangle(Position, Size);

        public List<Bullet> Bullets;
        public List<Enemy> Enemies;

        public Tree tree;

        public List<Sprite> Props = new List<Sprite>();

        public Player player;

        public GameStatePlaying Game;

        public Vector2 MouseWorldPos()
        {
            return Vector2.Transform(Input.MousePos, Matrix.Invert(Game.cam2d.GetViewMatrix()));
        }

        public Map(GameStatePlaying game)
        {
            Game = game;
            Size = new Point(10000, 100);
            Position = new Point(0, Globals.ScreenHeight - Size.Y);

            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();

            for(int i = 0; i < 30; i++)
            {
                var p = new Sprite();
                p.SetSize(32);
                p.Color = Color.DarkSeaGreen;
                p.Position = new Vector2(5 * (i * 36), Rectangle.Top - Rng.Noxt(16, 32));
                Props.Add(p);
            }

            tree = new Tree();
            tree.Position = new Vector2(GHelper.Center(Rectangle, tree.Size).X, Position.Y - tree.Size.Y);

            player = new Player();
            player.Position = new Vector2(GHelper.Center(Rectangle, player.Size).X, Position.Y - player.Size.Y);

        }

        public void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        public void Update(GameTime gt, GameScreen gs)
        {

            player.Update(gt, this, gs);


            for(int i = 0; i < Bullets.Count; i++)
            {
                var b = Bullets[i];
                b.Update(gt, this, gs);
            }

            Bullets.RemoveAll(x => x.LifeTime < 0);

            CheckCollision(player);
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

            tree.Draw(sb);

            player.Draw(sb);

            foreach(var b in Bullets)
            {
                b.Draw(sb);
            }

        }

    }
}
