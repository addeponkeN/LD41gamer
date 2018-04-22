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

    public class Recc
    {
        public Rectangle Rec;
        public bool IsPlatform;
        public Recc(Rectangle rec, bool isPlatform)
        {
            Rec = rec;
            IsPlatform = isPlatform;
        }
    }

    public class Map
    {
        public static float Gravity = 1500f;

        public Point GroundSize;
        public Point GroundPosition;

        public Rectangle GroundRectangle => new Rectangle(GroundPosition, GroundSize);
        public Rectangle GroundCollisionBox;
        public Rectangle BoxRectangle;

        public List<Bullet> Bullets;
        public List<Enemy> Enemies;
        public List<Turret> Turrets;

        public static ParticleEngine pengine;

        public Tree tree;

        public List<Sprite> Props = new List<Sprite>();

        public Player player;

        public GameStatePlaying Game;

        public Compass comp;

        public Builder builder;

        public Parlax parlax;

        float enemySpawnTimer = 4.5f;

        public List<Recc> CollisionBoxes;

        bool isInsideTree;

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

            GroundCollisionBox = new Rectangle(GroundRectangle.X, GroundRectangle.Y + 32, GroundRectangle.Width, GroundRectangle.Height);

            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();
            Turrets = new List<Turret>();
            CollisionBoxes = new List<Recc>();

            pengine = new ParticleEngine();

            comp = new Compass();

            builder = new Builder();

            //for(int i = 0; i < GroundRectangle.Right; i += 36)
            //{
            //    var p = new Sprite();
            //    p.SetSize(32);
            //    p.Color = Color.DarkSeaGreen;
            //    p.Position = new Vector2(i, GroundRectangle.Top - Rng.Noxt(-8, 64));
            //    Props.Add(p);
            //}

            tree = new Tree();
            tree.Position = new Vector2(GHelper.Center(GroundRectangle, tree.Size).X, GroundPosition.Y - tree.Size.Y + 53);

            foreach(TreeBranchType t in Enum.GetValues(typeof(TreeBranchType)))
            {
                tree.Add(t, this);
            }


            // -------------------------------
            //  create collision boxes

            for(int i = 0; i < tree.PlatformCollision.Count; i++)
            {
                CollisionBoxes.Add(new Recc(tree.PlatformCollision[i], true));
            }

            CollisionBoxes.Add(new Recc(GroundCollisionBox, false));

            // -------------------------------

            BoxRectangle = new Rectangle(0, 0, GroundRectangle.Right, GroundRectangle.Bottom);

            player = new Player();
            player.Position = new Vector2(GHelper.Center(GroundRectangle, player.Size).X, GroundPosition.Y - player.Size.Y);

            parlax = new Parlax(this);
        }

        public void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        public void Update(GameTime gt, GameScreen gs)
        {
            player.Update(gt, this, gs);
            parlax.Update(gt, this);
            pengine.Update(gt, this);

            for(int i = 0; i < Bullets.Count; i++)
            {
                var b = Bullets[i];
                b.Update(gt, this, gs);
            }

            for(int i = 0; i < Turrets.Count; i++)
            {
                var t = Turrets[i];
                t.Update(gt, this, gs);
                if(t.HealthPoints <= 0)
                    Turrets.Remove(t);
            }

            for(int i = 0; i < Enemies.Count; i++)
            {
                var e = Enemies[i];
                e.Update(gt, this, gs);

                for(int v = 0; v < Bullets.Count; v++)
                {
                    var b = Bullets[v];
                    if(e.Rectangle.Intersects(b.Rectangle))
                    {
                        //  enemy is hit
                        int blood = Rng.Noxt(3, 5);
                        for(int o = 0; o < blood; o++)
                        {
                            var pos = e.CollisionBox.Center() + new Vector2(Rng.Noxt(-16, 16), Rng.Noxt(-16, 16));
                            var dir = new Vector2(Rng.Noxt(-1, 1), -1);
                            var p = new Particle(ParticleType.Blood, pos, dir);
                            p.endPos = new Vector2(0, GroundCollisionBox.Top + p.Size.Y + Rng.Noxt(-20, 8));
                            pengine.Add(p);
                        }


                        e.HealthPoints--;
                        Bullets.Remove(b);
                    }
                }

                if(e.HealthPoints <= 0)
                {
                    player.Money += e.Reward;
                    e.IsAlive = false;
                }
            }

            for(int i = 0; i < tree.PlatformCollision.Count; i++)
            {
                var r = tree.PlatformCollision[i];
                player.PlatformCollision(r);
            }


            comp.Update(gt, this, player);

            Bullets.RemoveAll(x => x.LifeTime < 0);
            Enemies.RemoveAll(x => !x.IsAlive);

            enemySpawnTimer += gt.Delta();

            if(enemySpawnTimer >= 5)
            {
                SpawnEnemy();
                enemySpawnTimer = 0;
            }

            CheckCollision();


            tree.Update(gt, this, gs);
            builder.Update(gt, this);

            player.UpdateShooting(this, gt);


            if(player.Rectangle.Intersects(tree.HitBoxes[1]))
                isInsideTree = true;
            else
                isInsideTree = false;
        }

        public void SpawnEnemy()
        {
            var e = new Enemy(EnemyType.Wasp);

            //  e type
            if(Rng.Noxt(0, 1) == 0)
                e = new Enemy(EnemyType.Ant);

            //  side
            if(Rng.Noxt(0, 1) == 0)
                e.Position.X = GroundPosition.ToVector2().X - e.Size.X / 2;
            else
                e.Position.X = GroundRectangle.Width - e.Size.X / 2;

            //  flying
            if(e.IsFlying)
                e.Position.Y = Rng.Noxt(1450, 2330);
            else
                e.Position.Y = GroundPosition.Y - e.Size.Y / 2;


            Enemies.Add(e);
            comp.Add(e);
        }

        public void CheckCollision()
        {

            player.Collision(CollisionBoxes);

        }

        public void DrawWorld(SpriteBatch sb)
        {
            parlax.Draw(sb, this);

            //sb.Draw(GameContent.layer0, BoxRectangle, Color.White);
            //sb.Draw(GameContent.layer1, BoxRectangle, Color.White);
            //sb.Draw(GameContent.layer2, BoxRectangle, Color.White);
            //sb.Draw(GameContent.layer3, BoxRectangle, Color.White);

            for(int i = 0; i < GroundRectangle.Width; i += GameContent.ground.Width)
                sb.Draw(GameContent.ground, new Vector2(i, GroundPosition.Y), Color.White);

            foreach(var p in Props)
            {
                p.Draw(sb);
            }

            tree.Draw(sb, this);
            if(isInsideTree)
                sb.Draw(GameContent.treeInside, tree.Position, Color.White);

            foreach(var t in Turrets)
            {
                t.Draw(sb);
            }

            builder.Draw(sb);

            foreach(var e in Enemies)
            {
                e.Draw(sb);
                if(Globals.IsDebugging)
                    sb.Draw(UtilityContent.box, e.CollisionBox, Color.Red);
            }

            player.Draw(sb);

            foreach(var b in Bullets)
            {
                b.Draw(sb);
            }

            pengine.Draw(sb);

            if(!player.IsBuying)
                comp.Draw(sb);

            if(Globals.IsDebugging)
                foreach(var recc in CollisionBoxes)
                {
                    sb.Draw(UtilityContent.box, recc.Rec, Color.Red);
                }

        }

        public void DrawScreen(SpriteBatch sb)
        {
            //comp.Draw(sb);
        }

    }
}
