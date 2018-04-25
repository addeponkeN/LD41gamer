using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
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
    public enum TowerType
    {
        PeaShooter,
        AcornTurret,
        AcornSniper,
        ConeCatapult
    }

    public class Turret : LivingObject
    {
        public TowerType Type;

        public int Cost { get; set; }

        public float BuildTime;
        public float BuildTimeBase;

        public int Rank;

        public List<Sprite> stars;

        public bool isBeingBuilt;

        public Vector2 bulletStartPosLeft, bulletStartPosRight;

        AnimatedSprite blastCloud;
        public float blastTimer;

        public bool DrawShootBar;

        public Shape recf;
        Shape colLeft;
        Shape colRight;

        public static int TurretCost = 10;
        public static int SniperCost = 30;
        public static int CataCost = 100;

        public float DamageBase;

        public float Range => RangeBase + (Rank * 50);
        public float RangeBase;

        public float AttackSpeed => AttackSpeedBase / (((Rank + 1) * 0.05f) + 1);
        public float AttackSpeedBase;

        public float attackTimer;

        public bool shoot;
        public bool shot;
        public Vector2 target;

        float dmgLerp = 1f;

        HpBar ShootBar;
        HpBar ConBar;

        public bool isTargeted;
        public bool IsUpgrading;

        public bool ShowMenu;

        public bool DrawTowerInfo;

        float UpgradeTimer;
        public float UpgradeCooldownTimer;

        private float pTimer;

        public bool CanUpgrade => Rank < 4 && UpgradeCooldownTimer < 0f;

        float TimeToUpgrade => 2.33f + (Rank * 2f);

        public int UpgradeCost => (int)((Cost * 0.2f) + (((Rank + 1) * (Cost * 0.1f) * (Rank + 1))));
        float repairTimer;
        float repairTime = 1.5f;
        public bool CanRepair(Player player) => HealthPoints < MaxHealthPoints && player.Money >= repairCost;
        public int repairCost => (int)(2 + (Rank * 2f));
        public bool IsRepairing;

        public int Pierce => PierceBase;
        public int PierceBase;
        public Turret(TowerType t)
        {
            Type = t;
            Texture = UtilityContent.box;

            RangeBase = 700;
            AttackSpeedBase = 2;
            IsAnimating = false;

            SetSize(192);
            SetSourceSize(192);

            SetCollisionBot(115, 72);

            Texture = GameContent.turretsheet;

            bulletStartPosRight = new Vector2(154, 144);
            bulletStartPosLeft = new Vector2(37, 144);

            blastCloud = new AnimatedSprite();
            blastCloud.SetSize(96);
            blastCloud.SetSourceSize(221, 123);
            blastCloud.Texture = GameContent.blastcloud;
            blastCloud.PlayAnimation(AnimationType.BlastCloud);
            blastCloud.FrameLength = .1f;

            DrawLayer = Layer.Turret;

            switch(t)
            {


                case TowerType.PeaShooter:
                    Name = "Pea Shooter";
                    SetHp(5);
                    DamageBase = 1;
                    break;


                case TowerType.AcornTurret:
                    Name = "Acorn Turret";
                    SetHp(4);
                    Cost = 20;
                    DamageBase = 1;
                    BuildTimeBase = 2.5f;
                    SetFrame(0, 0);
                    break;


                case TowerType.AcornSniper:
                    Name = "Acorn Sniper";
                    SetHp(7);
                    Cost = 40;
                    DamageBase = 2;
                    AttackSpeedBase = 5;
                    RangeBase = 1100;
                    BuildTimeBase = 4f;
                    SetFrame(0, 1);
                    PierceBase = 1;
                    break;


                case TowerType.ConeCatapult:
                    Name = "Cone Catapult";
                    SetHp(15);
                    Cost = 100;
                    DamageBase = 3;
                    RangeBase = 2100;
                    BuildTimeBase = 6.5f;
                    SetFrame(0, 2);
                    AttackSpeedBase = 8f;
                    //SplashDamage = true;
                    IsAnimating = true;
                    PlayAnimation(AnimationType.CatapultIdle);

                    bulletStartPosRight = new Vector2(90, 105);
                    bulletStartPosLeft = new Vector2(90, 105);

                    break;

            }

            //BuildTimeBase = 0.5f;

            SetCols();


            recf = colRight;

            CreateBar();

            ShootBar = new HpBar((int)Size.X / 2, 3);
            ShootBar.gap = 1;
            ShootBar.Foreground.Color = Color.DeepSkyBlue;
            ShootBar.Background.Color = Color.MidnightBlue;

            ConBar = new HpBar((int)Size.X / 2, 3);
            ConBar.gap = 1;
            ConBar.Foreground.Color = Color.MonoGameOrange;
            ConBar.Background.Color = Color.DarkOrange;

            stars = new List<Sprite>();

        }

        void SetCols()
        {
            float Range2 = Range / 2;
            float Range4 = Range / 4;

            colRight = new Shape(
            Vector2.Zero,
            new Vector2(Range - Range4, -Range2),
            new Vector2(Range, 0),
            new Vector2(Range - Range4, Range2));

            colLeft = new Shape(
            Vector2.Zero,
            new Vector2(-Range + Range4, -Range2),
            new Vector2(-Range, 0),
            new Vector2(-Range + Range4, Range2));
        }

        public void Destroy(Map map)
        {
            SoundManager.PlaySound(GameSoundType.TowerDestroy);

            for(int k = 0; k < 20; k++)
            {
                var pos = CollisionBox.Center() /*+ new Vector2(Rng.Noxt(-16, 16), Rng.Noxt(-16, 16))*/;
                var dir = new Vector2(Rng.NoxtFloat(-1, 1), -1);
                var p = new Particle(ParticleType.Scrap, pos, dir);
                p.endPos = new Vector2(0, Map.GroundCollisionBox.Top + p.Size.Y + Rng.Noxt(-20, 8));
                Map.pengine.Add(p);
            }

            for(int h = 0; h < 70; h++)
            {
                var pos = CollisionBox.Center() + new Vector2(36, 28) + new Vector2(Rng.Noxt(-48, 48), Rng.Noxt(-48, 48));
                Map.pengine.Add(ParticleType.Smoke, pos, Particle.RandomDir());
            }
            IsAlive = false;
        }

        public void Upgrade(GameTime gt, Player player)
        {
            Damage = (int)(DamageBase + (Rank * 0.5f));
            var dt = gt.Delta();
            IsUpgrading = true;
            UpgradeTimer += dt;

            pTimer += dt;
            if(pTimer > 0.25)
            {
                var pos = CollisionBox.Center() + new Vector2(36, 28) + new Vector2(Rng.Noxt(-48, 48), Rng.Noxt(-48, 48));
                Map.pengine.Add(ParticleType.Smoke, pos, Particle.RandomDir());
                pTimer = 0;
            }

            if(UpgradeTimer > TimeToUpgrade)
            {
                player.Money -= UpgradeCost;
                UpgradeComplete();
            }

            //ShootBar.Foreground.Color = Color.LightGoldenrodYellow;
            //ShootBar.Background.Color = Color.DarkGoldenrod;
        }



        public void UpgradeComplete()
        {
            IsUpgrading = false;
            UpgradeCooldownTimer = 0.75f;
            Rank++;

            SetCols();

            HealthPoints = MaxHealthPoints;

            if(Type == TowerType.AcornSniper)
                PierceBase++;

            var s = new Sprite(GameContent.icons);
            s.SetSourceSize(64);
            s.SetFrame(0, 0);
            s.SetSize(20);

            stars.Add(s);
        }

        public void Repair(GameTime gt, Player player)
        {
            var dt = gt.Delta();
            IsRepairing = true;
            repairTimer += dt;

            pTimer += dt;
            if(pTimer > 0.20)
            {
                var pos = CollisionBox.Center() + new Vector2(36, 28) + new Vector2(Rng.Noxt(-48, 48), Rng.Noxt(-48, 48));
                Map.pengine.Add(ParticleType.Smoke, pos, Particle.RandomDir());
                pTimer = 0;
            }

            if(repairTimer > repairTime)
            {
                player.Money -= repairCost;
                RepairComplete();
            }

        }

        public void RepairComplete()
        {
            repairTimer = 0;
            IsRepairing = false;

            HealthPoints += 1;
        }

        public void UpdateRec()
        {
            if(SpriteEffects == SpriteEffects.None)
            {
                recf = colRight;
            }
            else
            {
                recf = colLeft;
            }

            recf.Position = CollisionBox.Center();
        }

        public void SetEffect(SpriteEffects ef)
        {
            SpriteEffects = ef;

        }


        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            var dt = gt.Delta();

            Damage = (int)(DamageBase + (Rank * 0.40));
            DrawHpBar = HealthPoints < MaxHealthPoints || isTargeted || DrawTowerInfo || Input.KeyHold(Keys.LeftShift) || isBeingBuilt;

            if(attackTimer < AttackSpeed)
                attackTimer += dt;
            blastTimer -= dt;

            UpgradeCooldownTimer -= dt;

            if(dmgLerp < 1f)
            {
                dmgLerp += dt;
                Color = Color.Lerp(Color.Red, BaseColor, dmgLerp);
            }

            if(Type != TowerType.ConeCatapult)
                blastCloud.UpdateAnimation(gt);

            if(IsUpgrading)
            {
                //ConBar.Foreground.Color = Color.LightGoldenrodYellow;
                //ConBar.Background.Color = Color.DarkGoldenrod;
                ConBar.Update(UpgradeTimer, TimeToUpgrade);
                attackTimer = 0;
            }
            else
                UpgradeTimer = 0f;

            if(IsRepairing)
            {
                //ConBar.Foreground.Color = Color.MonoGameOrange;
                //ConBar.Background.Color = Color.DarkOrange;
                ConBar.Update(repairTimer, repairTime);
            }
            else
                repairTimer = 0;

            if(DrawShootBar)
            {
                ShootBar.Update(attackTimer, AttackSpeed);
                ShootBar.Foreground.Color = Color.DeepSkyBlue;
                ShootBar.Background.Color = Color.MidnightBlue;
            }


            UpdateRec();


            if(Type == TowerType.ConeCatapult)
            {
                if(shoot)
                {
                    PlayAnimation(AnimationType.CatapultShoot);

                    if(frame >= 2 && !shot)
                    {
                        shot = true;
                        if(SpriteEffects == SpriteEffects.None)
                            Shoot(map, Position + bulletStartPosRight, target);
                        else
                            Shoot(map, Position + bulletStartPosLeft, target);

                    }

                    if(frame >= CurrentAnimation.Length - 1)
                    {
                        shot = false;
                        shoot = false;
                        PlayAnimation(AnimationType.CatapultIdle);
                        attackTimer = 0;
                    }

                }
            }
        }

        public void Shoot(Map map, Vector2 spawn, Vector2 target)
        {

            spawn = spawn - new Vector2(28 / 2);
            var dis = Vector2.Distance(spawn, target);
            var off = dis / 10;

            float extra;
            float extraRange;

            if(dis > 900)
            {
                off += off;
                extra = 300f;
                Console.WriteLine("LONGER RANGER");
            }

            off = Rng.Noxt((int)off - 20, (int)off + 20);

            Vector2 dest = target - new Vector2(0, off);
            Vector2 tCenter = CollisionBox.Center();

            float xBet = 0f;

            BulletType bulletType;

            if(Type == TowerType.ConeCatapult)
                bulletType = BulletType.Cone;
            else
                bulletType = BulletType.Acorn;


            if(Type == TowerType.ConeCatapult)
            {
                SoundManager.PlayFling();

                if(SpriteEffects == SpriteEffects.None)
                {
                    xBet = Center.X + (dis / 2);
                    //dest = new Vector2(tCenter.X + xBet, tCenter.Y);
                }
                else
                {
                    xBet = Center.X - (dis / 2);
                    // dest = new Vector2(tCenter.X - xBet, tCenter.Y);
                }

                int shots = Rng.Noxt(2, 4);
                map.AddBullet(new Bullet(bulletType, spawn, dest, Damage, Pierce, xBet));
                for(int j = 0; j < shots; j++)
                {
                    map.AddBullet(new Bullet(bulletType, spawn, dest + new Vector2(Rng.Noxt(-256, 256), Rng.Noxt(-256, 256)), Damage, Pierce, xBet));
                }
            }
            else
            {
                SoundManager.PlayTowerShoot();

                map.AddBullet(new Bullet(bulletType, spawn, dest, Damage, Pierce));
            }

            int i = map.Bullets.Count - 1;

            if(SpriteEffects == SpriteEffects.None)
                map.Bullets[i].Position.X -= 10;
            else
                map.Bullets[i].Position.X += 10;

            blastCloud.frame = 0;
            if(SpriteEffects == SpriteEffects.None)
            {
                blastCloud.SpriteEffects = SpriteEffects.FlipHorizontally;
                blastCloud.Position = GHelper.Center(spawn, blastCloud.Size) + new Vector2(13, 6);
            }
            else
                blastCloud.Position = GHelper.Center(spawn, blastCloud.Size) + new Vector2(13, 6);

            blastTimer = (float)blastCloud.AnimationDuration - .1f;
        }

        public void IsHit(int damage)
        {
            HealthPoints -= damage;
            dmgLerp = 0.5f;

            SoundManager.PlayTowerHit();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);


            if(IsUpgrading)
            {
                var hamSize = new Vector2(64);
                Builder.DrawHammer(sb, new Vector2(Center.X - hamSize.X / 2, CollisionBox.Top - hamSize.Y - 50), hamSize);
            }

            if(DrawTowerInfo || Input.KeyHold(Keys.LeftShift) || isTargeted)
            {
                for(int i = 0; i < stars.Count; i++)
                {
                    var s = stars[i];
                    s.Position = HpBar.Position - new Vector2((i * -s.Size.X) + (i * -6), s.Size.Y + 10);
                    s.Draw(sb);
                }

                //HpBar.Draw(sb, new Vector2(GHelper.Center(Rectangle, HpBar.Size).X, CollisionBox.Top - 32));
            }

            if(DrawShootBar)
            {
                if(IsUpgrading || attackTimer < AttackSpeed || isTargeted)
                    ShootBar.Draw(sb, new Vector2(GHelper.Center(HpBar.Rectangle, ShootBar.Size).X, HpBar.Rectangle.Top - ShootBar.Size.Y - 4));
            }

            if(IsUpgrading || IsRepairing)
            {
                ConBar.Draw(sb, new Vector2(GHelper.Center(HpBar.Rectangle, ShootBar.Size).X, HpBar.Rectangle.Top - ShootBar.Size.Y - 10));
            }

            if(Type != TowerType.ConeCatapult)
                if(blastTimer >= 0)
                    blastCloud.Draw(sb);

            DrawShadow(sb, -8f);

        }

        public void DrawRange(SpriteBatch sb)
        {
            recf.Draw(sb, Color.LightBlue);
        }

    }
}
