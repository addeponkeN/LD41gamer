using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
using ld41gamer.Gamer.StateMachine.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public static Rectangle GroundCollisionBox;
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

        public GameStater GameState = GameStater.Level1_0;

        float enemySpawnTimer = 1f;
        float enemySpawnCd = 5f;

        public float holeSpawnTimer = 0f;
        public float holeSpawnCd = 50f;
        public bool holeSpawn = false;

        public float beavSpawnTimer = 0f;
        public float beavSpawnCd = 50;
        public bool beavSpawn = false;

        bool lastWave;

        public float GameTimer;

        public List<Recc> CollisionBoxes;

        bool isInsideTree;

        float insideLerp = 1f;

        public static int WallLeft = 760;
        public static int WallRight = 9900;

        public static Vector2 PlayerSpawnPos;
        public static Upgrades Upgrades;

        public static float FadeLerp = 1f;
        public Sprite fadeBox;

        public Turret closestTurret;

        public TreeHp treeBar;

        public Vector2 MouseWorldPos()
        {
            return Vector2.Transform(Input.MousePos, Matrix.Invert(Game.cam2d.GetViewMatrix()));
        }

        public Map(GameStatePlaying game)
        {
            Game = game;
            GroundSize = new Point(10000, 180);
            //GroundPosition = new Point(0, Globals.ScreenHeight - GroundSize.Y);
            GroundPosition = new Point(0, 2500);

            GroundCollisionBox = new Rectangle(GroundRectangle.X, GroundRectangle.Y + 32, GroundRectangle.Width, GroundRectangle.Height);

            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();
            Turrets = new List<Turret>();
            CollisionBoxes = new List<Recc>();
            Upgrades = new Upgrades();

            pengine = new ParticleEngine();

            comp = new Compass();

            builder = new Builder();

            fadeBox = new Sprite(UtilityContent.box);
            fadeBox.Size = Globals.ScreenSize;
            fadeBox.Position = new Vector2(0);

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
            PlayerSpawnPos = new Vector2(GHelper.Center(GroundRectangle, player.Size).X, GroundCollisionBox.Y - player.Size.Y);
            player.Position = PlayerSpawnPos;

            parlax = new Parlax(this);

            treeBar = new TreeHp();
            treeBar.Posser(new Vector2(8, 8));

            game.cam2d.LookAt(player.Center);
            game.LockCamToMap(this);
        }

        public void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        public void Update(GameTime gt, GameScreen gs)
        {
            var dt = gt.Delta();

            GameTimer += dt;

            CheckGameState();

            player.Update(gt, this, gs);
            parlax.Update(gt, this);
            pengine.Update(gt, this);

            UpdateSpawning(gt);

            treeBar.Update(tree.HealthPoints, tree.MaxHealthPoints);



            for(int i = 0; i < Bullets.Count; i++)
            {
                var b = Bullets[i];
                b.Update(gt, this, gs);
            }




            //  TURRET UPDATE
            float d = 999999f;
            for(int i = 0; i < Turrets.Count; i++)
            {
                var t = Turrets[i];
                t.Update(gt, this, gs);
                if(t.HealthPoints <= 0 || !t.IsAlive)
                {

                    t.Destroy(this);
                    if(closestTurret == t)
                    {
                        player.IsUpgradingOrReparing = false;
                        SoundManager.StopLoop(GameSoundType.TowerBuilding);
                        closestTurret = null;
                    }

                    Turrets.RemoveAt(i);
                    continue;
                }

                //if(Input.KeyClick(Keys.C))
                //    t.HealthPoints--;

                t.isTargeted = false;
                if(player.IsAlive)
                {
                    var dis = Vector2.Distance(t.CollisionBox.Center(), player.CollisionBox.Center());
                    if(d > dis)
                    {
                        d = dis;
                        closestTurret = t;
                    }
                }
            }




            //  CLOSEST TURRET
            if(closestTurret != null && player.IsAlive)
                if(player.CollisionBox.Intersects(closestTurret.CollisionBox))
                {
                    closestTurret.isTargeted = true;

                    closestTurret.IsUpgrading = false;

                    closestTurret.IsRepairing = false;

                    player.IsUpgradingOrReparing = false;

                    //Builder.RepairCost = 10;
                    //Builder.UpgradeCost = closestTurret.UpgradeCost;

                    if(player.Money >= closestTurret.UpgradeCost)
                        if(closestTurret.CanUpgrade)
                            if(Input.KeyHold(Keys.G))
                            {
                                closestTurret.Upgrade(gt, player);
                                player.IsUpgradingOrReparing = true;
                            }

                    if(player.Money >= closestTurret.repairCost)
                        if(closestTurret.CanRepair(player))
                            if(Input.KeyHold(Keys.R))
                            {
                                closestTurret.Repair(gt, player);
                                player.IsUpgradingOrReparing = true;
                            }

                    if(Input.KeyClick(Keys.Q))
                    {
                        closestTurret.IsAlive = false;
                    }

                }





            //  BIG ENEMY UPDATE
            for(int i = 0; i < Enemies.Count; i++)
            {
                var e = Enemies[i];
                e.Update(gt, this, gs);
                if(player.IsAlive && e.Type != EnemyType.WormHole)
                    if(e.CollisionBox.Intersects(player.CollisionBox))
                    {
                        int dir = 0;
                        if(e.SpriteEffects == SpriteEffects.None)
                            dir = 1;
                        else
                            dir = -1;


                        //###################
                        //######## PLAYER DIE HERE
                        //############ comment.



                        player.Die(dir);



                        //########## easy find
                        //#########################                        

                        player.dmgLerp = 0.5f;
                    }

                for(int v = 0; v < Bullets.Count; v++)
                {
                    var b = Bullets[v];
                    if(!b.hits.Any(h => i == h))
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
                                e.dmgLerp = 0.5f;
                            }
                            SoundManager.PlayEnemyHit();
                            e.HealthPoints -= b.Damage;
                            if(b.Pierce <= 0)
                                Bullets.Remove(b);
                            else
                            {
                                b.Pierce--;
                                b.hits.Add(i);
                            }
                        }
                }
                bool attack = false;
                for(int j = 0; j < Turrets.Count; j++)
                {
                    var t = Turrets[j];

                    //  enemy attack turret
                    if(e.CollisionBox.Intersects(t.CollisionBox) && e.Type != EnemyType.WormHole)
                    {
                        e.attackTimer += dt;
                        if(e.attackTimer >= e.attackCooldown)
                        {
                            t.IsHit(e.Damage);
                            e.attackTimer = 0;
                        }
                        attack = true;
                    }


                    if(!t.IsUpgrading)
                    {
                        bool tryShoot = false;
                        //  looking right
                        if(t.SpriteEffects == SpriteEffects.None)
                        {
                            if(t.Type == TowerType.ConeCatapult)
                            {
                                if(e.Rectangle.Left > t.Center.X + 300)
                                    tryShoot = true;
                            }
                            else if(e.Rectangle.Left > t.Center.X)
                                tryShoot = true;
                        }
                        else
                        {
                            if(t.Type == TowerType.ConeCatapult)
                            {
                                if(e.Rectangle.Right < t.Center.X - 300)
                                    tryShoot = true;
                            }
                            else
                            if(e.Rectangle.Right < t.Center.X)
                                tryShoot = true;
                        }

                        if(!tryShoot)
                            continue;

                        if(t.attackTimer >= t.AttackSpeed)
                        {
                            if(t.recf.Intersects(e.CollisionBox))
                            {
                                t.target = e.Center;
                                if(t.Type != TowerType.ConeCatapult)
                                {
                                    if(t.SpriteEffects == SpriteEffects.None)
                                        t.Shoot(this, t.Position + t.bulletStartPosRight, t.target);
                                    else
                                        t.Shoot(this, t.Position + t.bulletStartPosLeft, t.target);

                                    t.attackTimer = 0;
                                }
                                else
                                    t.shoot = true;
                            }
                        }
                    }
                }

                e.isAttacking = attack;


                if(e.HealthPoints <= 0)
                {
                    player.Money += e.Reward;
                    e.IsAlive = false;
                }
            }

            comp.Update(gt, this, player);

            Bullets.RemoveAll(x => x.LifeTime < 0);
            Enemies.RemoveAll(x => !x.IsAlive);

            //if(Input.KeyClick(Keys.P))
            //    SpawnEnemy(Enemy.RandomTypeNotWormHole());
            //if(Input.KeyClick(Keys.K))
            //    SpawnWormHole(true);

            CheckCollision();


            tree.Update(gt, this, gs);
            builder.Update(gt, this);

            player.UpdateShooting(this, gt);


            if(player.Rectangle.Intersects(tree.HitBoxes[1]))
                isInsideTree = true;
            else
                isInsideTree = false;


            if(isInsideTree)
            {
                if(insideLerp < 1f)
                    insideLerp += dt * 1.5f;
            }
            else
            {
                if(insideLerp > 0f)
                    insideLerp -= dt * 2f;
            }



            if(tree.HealthPoints <= 0)
            {
                Game.game.AddPopupScreen(new PopupScreen($"" +
                    $"\n      The tree is dead!" +
                    $"\nYou survived for: {Game.time.ToString("mm':'ss")}", GameContent.font24, GameContent.bigplank, GameContent.btplank, new Color(200, 200, 200), 300, 175, GameContent.font24, PopupType.Ok), true);
            }


        }

        public enum Side
        {
            None,
            Left,
            Right
        }

        public void CheckGameState()
        {
            if(GameTimer > 760)
                GameState = GameStater.Level11_740;

            else if(GameTimer > 670)
                GameState = GameStater.Level10_670;

            else if(GameTimer > 630)
                GameState = GameStater.Level9_630_break;

            else if(GameTimer > 550)
                GameState = GameStater.Level9_550;

            else if(GameTimer > 500)
                GameState = GameStater.Level8_500_Break;

            else if(GameTimer > 420)
                GameState = GameStater.Level8_420;

            else if(GameTimer > 300)
                GameState = GameStater.Level7_300;

            else if(GameTimer > 210)
                GameState = GameStater.Level6_210;

            else if(GameTimer > 150)
                GameState = GameStater.Level5_150;

            else if(GameTimer > 90)
                GameState = GameStater.Level4_90;

            else if(GameTimer > 50)
                GameState = GameStater.Level3_50;

            else if(GameTimer > 20)
                GameState = GameStater.Level2_20;

            else
                GameState = GameStater.Level1_0;
        }

        public void SpawnEnemy(EnemyType type, Side side = Side.None)
        {
            var e = new Enemy(type);

            //MessagePopupManager.AddMsg("Spawned: " + type.ToString() + "    next in: " + (int)enemySpawnCd, false);

            if(side == Side.None)
            {
                if(Rng.NextBool)
                    side = Side.Left;
                else
                    side = Side.Right;
            }

            if(side == Side.Left)
                e.Position.X = Map.WallLeft - Rng.Noxt(0, 200) - e.Size.X / 2;
            else
                e.Position.X = Map.WallRight + Rng.Noxt(0, 200) - e.Size.X / 2;

            if(e.IsFlying)
                e.Position.Y = Rng.Noxt(1450, 2330);
            else
                e.Position.Y = GroundCollisionBox.Top - e.Size.Y;

            Enemies.Add(e);
            comp.Add(e);
        }

        bool rng(int i) => Rng.Next(100) < i;

        public int spawnTicks = 0;
        public void UpdateSpawning(GameTime gt)
        {
            var dt = gt.Delta();

            int e;

            enemySpawnTimer += dt;
            holeSpawnTimer += dt;
            beavSpawnTimer += dt;

            if(enemySpawnTimer >= enemySpawnCd)
            {
                spawnTicks++;
                switch(GameState)
                {


                    case GameStater.Level1_0:
                        enemySpawnCd = 5;
                        SpawnEnemy(EnemyType.WormYellow, Side.Left);
                        SpawnEnemy(EnemyType.WormYellow, Side.Right);

                        break;


                    case GameStater.Level2_20:
                        enemySpawnCd = 6.5f;
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);

                        //SpawnEnemy(Enemy.GetRandomType(
                        //    EnemyType.WormYellow,
                        //    EnemyType.WormBlue));
                        break;


                    case GameStater.Level3_50:
                        enemySpawnCd = 5.5f;
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);

                        //SpawnEnemy(Enemy.GetRandomType(
                        //    EnemyType.WormYellow,
                        //    EnemyType.WormBlue));
                        break;


                    case GameStater.Level4_90:

                        enemySpawnCd = 6;
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormYellow,
                            EnemyType.WormBlue));

                        if(rng(15))
                            SpawnEnemy(EnemyType.Wasp);

                        break;


                    case GameStater.Level5_150:


                        enemySpawnCd = 5;
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant));

                        if(rng(15))
                            SpawnEnemy(EnemyType.Wasp);

                        break;


                    case GameStater.Level6_210:

                        enemySpawnCd = 4.5f;

                        //beavSpawnCd = 211;
                        //SpawnBeaver();

                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant));

                        if(rng(20)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(10))
                            SpawnEnemy(EnemyType.WormRed);

                        break;


                    case GameStater.Level7_300:

                        enemySpawnCd = Rng.Noxt(4, 6);
                        SpawnEnemy(EnemyType.WormYellow);
                        SpawnEnemy(EnemyType.WormYellow);

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        if(rng(25)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);

                        break;

                    case GameStater.Level8_420:

                        beavSpawnCd = Rng.Noxt(40, 60);
                        holeSpawnCd = Rng.Noxt(25, 35);

                        enemySpawnCd = Rng.Noxt(3, 5);

                        SpawnEnemy(EnemyType.WormYellow);

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        if(rng(65)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(20)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);

                        SpawnWormHole();
                        SpawnBeaver();

                        break;


                    //  break
                    case GameStater.Level8_500_Break:

                        beavSpawnCd = Rng.Noxt(500, 600);
                        holeSpawnCd = Rng.Noxt(500, 600);

                        enemySpawnCd = Rng.Noxt(5, 6);


                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormYellow));
                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormYellow,
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        if(rng(10)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);

                        SpawnWormHole();

                        if(rng(50))
                            SpawnBeaver();

                        break;


                    case GameStater.Level9_550:
                        beavSpawnCd = Rng.Noxt(25, 35);
                        holeSpawnCd = Rng.Noxt(20, 30);
                        //beavSpawnCd = 40;
                        //holeSpawnCd = 60;
                        enemySpawnCd = Rng.Noxt(4, 6);

                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormYellow));
                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormBlue));

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        if(rng(55)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(45)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);


                        if(rng(50))
                            SpawnEnemy(EnemyType.WormBlue);

                        if(rng(30))
                            SpawnEnemy(EnemyType.Ant);


                        SpawnBeaver();

                        SpawnWormHole();


                        break;

                    case GameStater.Level9_630_break:

                        beavSpawnCd = Rng.Noxt(150, 160);
                        holeSpawnCd = Rng.Noxt(50, 60);
                        //beavSpawnCd = 40;
                        //holeSpawnCd = 60;
                        enemySpawnCd = Rng.Noxt(5, 6);

                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormYellow));
                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormYellow));
                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormYellow));

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue));

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue));

                        if(rng(35)) // 25%                        
                            SpawnEnemy(EnemyType.Wasp);


                        if(rng(15))
                            SpawnEnemy(EnemyType.WormBlue);

                        if(rng(10))
                            SpawnEnemy(EnemyType.Ant);


                        SpawnBeaver();

                        SpawnWormHole();

                        break;


                    case GameStater.Level10_670:

                        beavSpawnCd = Rng.Noxt(25, 30);
                        holeSpawnCd = Rng.Noxt(20, 25);

                        enemySpawnCd = Rng.Noxt(5, 6);

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        if(rng(80))
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(25))
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(50))
                            SpawnEnemy(EnemyType.WormBlue);

                        if(rng(30))
                            SpawnEnemy(EnemyType.Ant);


                        if(rng(80))
                            SpawnEnemy(EnemyType.WormYellow);

                        if(rng(50))
                            SpawnEnemy(EnemyType.WormYellow);

                        SpawnBeaver();
                        SpawnWormHole();

                        if(rng(20))
                            SpawnBeaver();

                        break;

                    case GameStater.Level11_740:

                        if(!lastWave)
                        {
                            beavSpawnCd = 25;
                            holeSpawnCd = 15;
                        }
                        else
                        {
                            if(beavSpawnCd > 5)
                                beavSpawnCd -= 0.5f;

                            if(holeSpawnCd > 5)
                                holeSpawnCd -= 0.5f;

                            Enemy.HpIncreaser += 0.1f;
                        }


                        enemySpawnCd = Rng.Noxt(2, 3);

                        SpawnEnemy(Enemy.GetRandomType(EnemyType.WormYellow, EnemyType.WormBlue));

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        SpawnEnemy(Enemy.GetRandomType(
                            EnemyType.WormBlue,
                            EnemyType.Ant,
                            EnemyType.WormRed));

                        if(rng(90))
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(50))
                            SpawnEnemy(EnemyType.Wasp);

                        if(rng(50))
                            SpawnEnemy(EnemyType.WormBlue);

                        if(rng(30))
                            SpawnEnemy(EnemyType.Ant);

                        if(rng(50))
                            SpawnEnemy(EnemyType.WormYellow);

                        if(rng(50))
                            SpawnEnemy(EnemyType.WormYellow);

                        SpawnBeaver();
                        SpawnWormHole();


                        lastWave = true;
                        break;


                }

                enemySpawnTimer = 0;
            }

            //if(holeSpawnTimer > holeSpawnCd)
            //{
            //    SpawnWormHole();
            //    holeSpawnTimer = 0;
            //}

        }

        private void SpawnWormHole(bool ignoreCd = false)
        {
            if(!ignoreCd)
            {
                if(holeSpawnTimer < holeSpawnCd)
                {
                    //MessagePopupManager.AddMsg("NOT SPAWNING WormHole:  CD: " + (int)holeSpawnTimer + " / " + (int)holeSpawnCd, false);
                    return;
                }
                holeSpawnTimer = 0;
            }

            var e = new Enemy(EnemyType.WormHole);

            if(Rng.NextBool)
                e.Position.X = Rng.Noxt(WallLeft + 1000, tree.BottomHitBox.Left - 1300);
            else
                e.Position.X = Rng.Noxt(tree.BottomHitBox.Right + 1300, WallRight - 1000);

            e.Position.Y = GroundCollisionBox.Top - e.Size.Y;

            //MessagePopupManager.AddMsg("Spawned: " + EnemyType.WormHole.ToString() + "    next in: " + (int)enemySpawnCd, false);

            Enemies.Add(e);
            comp.Add(e);
        }

        void SpawnBeaver()
        {

            if(beavSpawnTimer < beavSpawnCd)
            {
                //MessagePopupManager.AddMsg("NOT SPAWNING Beaver:  CD: " + (int)beavSpawnTimer + " / " + (int)beavSpawnCd, false);
                return;
            }
            beavSpawnTimer = 0;


            var e = new Enemy(EnemyType.Beaver);

            //MessagePopupManager.AddMsg("Spawned: " + EnemyType.Beaver.ToString() + "    next in: " + (int)enemySpawnCd, false);

            if(Rng.NextBool)
                e.Position.X = Map.WallLeft - Rng.Noxt(0, 200) - e.Size.X / 2;
            else
                e.Position.X = Map.WallRight + Rng.Noxt(0, 200) - e.Size.X / 2;

            e.Position.Y = GroundCollisionBox.Top - e.Size.Y;

            Enemies.Add(e);
            comp.Add(e);
        }

        public void SpawnWorm(int posx)
        {
            var e = new Enemy(EnemyType.WormYellow);
            e.Position = new Vector2(posx, GroundCollisionBox.Top - e.Size.Y);
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

            //sb.Draw(GameContent.layer0, BoxRectangle, Color.White, Layer.BACK + 0.10f);
            //sb.Draw(GameContent.layer1, BoxRectangle, Color.White, Layer.BACK + 0.11f);
            //sb.Draw(GameContent.layer2, BoxRectangle, Color.White, Layer.BACK + 0.12f);
            //sb.Draw(GameContent.layer3, BoxRectangle, Color.White, Layer.BACK + 0.13f);

            for(int i = 0; i < GroundRectangle.Width; i += GameContent.ground.Width)
                sb.Draw(GameContent.ground, new Rectangle(i, (int)GroundPosition.Y, GameContent.ground.Width, GameContent.ground.Height), Color.White, Layer.Ground);

            foreach(var p in Props)
            {
                p.Draw(sb);
            }

            tree.Draw(sb, this);

            int alp = (int)MathHelper.Lerp(0, 255, insideLerp);

            sb.Draw(GameContent.treeInside, tree.Position, new Color(alp, alp, alp, alp), Layer.TreeInside);

            foreach(var t in Turrets)
            {
                t.Draw(sb);
                if(player.IsShopping || Builder.IsPlacing)
                    t.DrawRange(sb);
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

            if(!player.IsShopping)
                comp.Draw(sb);

            if(Globals.IsDebugging)
                foreach(var recc in CollisionBoxes)
                {
                    sb.Draw(UtilityContent.box, recc.Rec, Color.Red);
                }

        }

        public void DrawTowerRecs(SpriteBatch sb)
        {
            foreach(var t in Turrets)
            {
                if(player.IsShopping || Builder.IsPlacing)
                    t.DrawRange(sb);
            }

            if(player.IsBuilding || Builder.IsPlacing)
                builder.DrawRecs(sb);
        }


        public void DrawDef(SpriteBatch sb)
        {

            if(closestTurret != null)
                if(closestTurret.isTargeted && !closestTurret.IsRepairing && !closestTurret.IsUpgrading)
                {
                    var size = new Vector2(48);
                    var pos = new Vector2(GHelper.Center(closestTurret.Rectangle, size).X - size.X + 14, closestTurret.Position.Y - 15);
                    Builder.DrawUpgradeAndRepair(sb, this, pos, size, closestTurret.Rank < 4);
                }

            if(tree.CanBench)
            {
                Builder.DrawBenchUpgrade(sb, tree.BenchRec.TopMiddle() - new Vector2(7, 100), new Vector2(48));
            }


            foreach(var t in builder.Con)
            {
                if(t.isBeingBuilt)
                    continue;
                var hamSize = new Vector2(48);
                //DrawHammer(sb, new Vector2(t.Center.X - hamSize.X / 2, t.CollisionBox.Top - hamSize.Y - 55), hamSize);
                Builder.DrawHam(sb, new Vector2(t.Center.X - hamSize.X / 2, t.CollisionBox.Top - hamSize.Y - 55), new Vector2(48));
            }
            //if(!player.IsBuilding)
            //    if(Builder.CanBuildPos != Vector2.Zero)
            //    {
            //        Builder.DrawHam(sb, Builder.CanBuildPos, new Vector2(48));
            //    }


            treeBar.Draw(sb, treeBar.Position);

        }

        public void DrawScreen(SpriteBatch sb)
        {
            //fadeBox.Color = Color.Lerp(Color.Black, Color.White, FadeLerp);
            fadeBox.Color = Color.Black;
            fadeBox.Alpha = (int)MathHelper.Lerp(255, 0, FadeLerp);
            //fadeBox.Alpha = 255;
            fadeBox.Draw(sb);
        }



    }
}
