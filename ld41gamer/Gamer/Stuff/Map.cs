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

        public Point GroundSize;
        public Point GroundPosition;

        public Rectangle GroundRectangle => new Rectangle(GroundPosition, GroundSize);
        public Rectangle BoxRectangle;

        public List<Bullet> Bullets;
        public List<Enemy> Enemies;

        public Tree tree;

        public List<Sprite> Props = new List<Sprite>();

        public Player player;

        public GameStatePlaying Game;

        public Compass comp;

        float enemySpawnTimer = 4.5f;

        public Vector2 MouseWorldPos()
        {
            return Vector2.Transform(Input.MousePos, Matrix.Invert(Game.cam2d.GetViewMatrix()));
        }

        public Map(GameStatePlaying game)
        {
            Game = game;
            GroundSize = new Point(10000, 100);
            //GroundPosition = new Point(0, Globals.ScreenHeight - GroundSize.Y);
            GroundPosition = new Point(0, 2500);

            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();

            comp = new Compass();

            for(int i = 0; i < 30; i++)
            {
                var p = new Sprite();
                p.SetSize(32);
                p.Color = Color.DarkSeaGreen;
                p.Position = new Vector2(5 * (i * 36), GroundRectangle.Top - Rng.Noxt(16, 32));
                Props.Add(p);
            }

            tree = new Tree();
            tree.Position = new Vector2(GHelper.Center(GroundRectangle, tree.Size).X, GroundPosition.Y - tree.Size.Y + 53);

            BoxRectangle = new Rectangle(0, 0, GroundRectangle.Right, GroundRectangle.Bottom);

            player = new Player();
            player.Position = new Vector2(GHelper.Center(GroundRectangle, player.Size).X, GroundPosition.Y - player.Size.Y);

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

            for(int i = 0; i < Enemies.Count; i++)
            {
                var e = Enemies[i];
                e.Update(gt, this, gs);
            }

            comp.Update(gt, Game.cam2d, player);

            Bullets.RemoveAll(x => x.LifeTime < 0);
            Enemies.RemoveAll(x => !x.IsAlive);

            CheckCollision(player);

            enemySpawnTimer += gt.Delta();

            if(enemySpawnTimer >= 5)
            {
                SpawnEnemy();
                enemySpawnTimer = 0;
            }
        }

        public void SpawnEnemy()
        {
            var e = new Enemy(EnemyType.Ant);

            var side = Rng.Noxt(0, 1);            
            if(side == 0)
                e.Position = GroundPosition.ToVector2();
            else
                e.Position = new Vector2(GroundRectangle.Width, GroundPosition.Y);

            Enemies.Add(e);
            comp.Add(e);
        }

        public void CheckCollision(Player p)
        {
            p.Collision(new Rectangle(GroundRectangle.X, GroundRectangle.Y + 32, GroundRectangle.Width, GroundRectangle.Height));
        }

        public void DrawWorld(SpriteBatch sb)
        {
            sb.Draw(GameContent.layer3, BoxRectangle, Color.White);
            sb.Draw(GameContent.layer2, BoxRectangle, Color.White);
            sb.Draw(GameContent.layer1, BoxRectangle, Color.White);
            sb.Draw(GameContent.layer0, BoxRectangle, Color.White);

            for(int i = 0; i < GroundRectangle.Width; i += GameContent.ground.Width)
                sb.Draw(GameContent.ground, new Vector2(i, GroundPosition.Y), Color.White);

            foreach(var p in Props)
            {
                p.Draw(sb);
            }

            foreach(var e in Enemies)
            {
                e.Draw(sb);
            }

            tree.Draw(sb);

            player.Draw(sb);

            foreach(var b in Bullets)
            {
                b.Draw(sb);
            }

            comp.Draw(sb);

        }

        public void DrawScreen(SpriteBatch sb)
        {
            //comp.Draw(sb);
        }

    }
}
