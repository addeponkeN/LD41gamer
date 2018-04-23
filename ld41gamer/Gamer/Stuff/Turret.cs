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

        public float attackTimer;
        public float AttackSpeed;

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

        //  range in pixels
        public int Range;

        public bool shoot;
        public bool shot;
        public Vector2 target;

        float dmgLerp = 1f;

        HpBar ShootBar;

        public bool isTargeted;
        public bool IsUpgrading;

        public bool ShowMenu;

        public bool DrawTowerInfo;

        float UpgradeTimer;
        public float UpgradeCooldownTimer;

        private float pTimer;

        public bool CanUpgrade => Rank <= 4 && UpgradeCooldownTimer < 0f;

        float TimeToUpgrade => 3f + (Rank * 2f);

        float repairTimer;
        public bool CanRepair(Player player) => HealthPoints < MaxHealthPoints && player.Money >= 10;
        public bool IsRepairing;

        public Turret(TowerType t)
        {
            Type = t;
            Texture = UtilityContent.box;

            Range = 700;
            AttackSpeed = 2;
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
                    Damage = 1;
                    break;


                case TowerType.AcornTurret:
                    Name = "Acorn Turret";
                    SetHp(4);
                    Cost = 10;
                    Damage = 1;
                    BuildTimeBase = 4f;
                    SetFrame(0, 0);
                    break;


                case TowerType.AcornSniper:
                    Name = "Acorn Sniper";
                    SetHp(4);
                    Cost = 30;
                    Damage = 3;
                    AttackSpeed = 5;
                    Range = 1200;
                    BuildTimeBase = 7f;
                    SetFrame(0, 1);
                    break;


                case TowerType.ConeCatapult:
                    Name = "Cone Catapult";
                    SetHp(15);
                    Cost = 100;
                    Damage = 3;
                    Range = 1800;
                    BuildTimeBase = 15f;
                    SetFrame(0, 0);
                    AttackSpeed = 8f;
                    //SplashDamage = true;
                    IsAnimating = true;
                    PlayAnimation(AnimationType.CatapultIdle);

                    bulletStartPosRight = new Vector2(90, 105);
                    bulletStartPosLeft = new Vector2(90, 105);

                    break;

            }

            BuildTimeBase = 0.5f;

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


            recf = colRight;

            CreateBar();

            ShootBar = new HpBar((int)Size.X / 2, 3);
            ShootBar.gap = 1;
            ShootBar.Foreground.Color = Color.DeepSkyBlue;
            ShootBar.Background.Color = Color.MidnightBlue;

            stars = new List<Sprite>();

        }

        public void Upgrade(GameTime gt)
        {
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
                UpgradeComplete();
            }

            ShootBar.Foreground.Color = Color.LightGoldenrodYellow;
            ShootBar.Background.Color = Color.DarkGoldenrod;
        }



        public void UpgradeComplete()
        {
            IsUpgrading = false;
            UpgradeCooldownTimer = 1.5f;
            Rank++;

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
            if(pTimer > 0.25)
            {
                var pos = CollisionBox.Center() + new Vector2(36, 28) + new Vector2(Rng.Noxt(-48, 48), Rng.Noxt(-48, 48));
                Map.pengine.Add(ParticleType.Smoke, pos, Particle.RandomDir());
                pTimer = 0;
            }

            if(repairTimer > 2)
            {
                player.Money -= 10;
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

            DrawHpBar = HealthPoints < MaxHealthPoints ||  isTargeted;

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
                ShootBar.Foreground.Color = Color.LightGoldenrodYellow;
                ShootBar.Background.Color = Color.DarkGoldenrod;
                ShootBar?.Update(UpgradeTimer, TimeToUpgrade);
            }
            else
                UpgradeTimer = 0f;

            if(!IsRepairing)
                repairTimer = 0f;

            if(DrawShootBar && !IsUpgrading)
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


            if(dis > 700)
            {
                off += off;
                Console.WriteLine("LONGER RANGER");
            }

            off = Rng.Noxt((int)off - 20, (int)off + 20);

            Vector2 dest = target - new Vector2(0, off);
            Vector2 tCenter = CollisionBox.Center();

            float xBet;

            BulletType bulletType;

            if(Type == TowerType.ConeCatapult)
                bulletType = BulletType.Cone;
            else
                bulletType = BulletType.Acorn;

            if(Type == TowerType.ConeCatapult)
            {

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
                map.AddBullet(new Bullet(bulletType, spawn, dest, Damage, xBet));
                for(int j = 0; j < shots; j++)
                {
                    map.AddBullet(new Bullet(bulletType, spawn, dest + new Vector2(Rng.Noxt(-256, 256), Rng.Noxt(-256, 256)), Damage, xBet));
                }
            }
            else
                map.AddBullet(new Bullet(bulletType, spawn, dest, Damage));



            int i = map.Bullets.Count - 1;

            if(SpriteEffects == SpriteEffects.None)
                map.Bullets[i].Position.X -= 10;
            else
                map.Bullets[i].Position.X += 10;

            blastCloud.frame = 0;
            if(SpriteEffects == SpriteEffects.None)
            {
                blastCloud.SpriteEffects = SpriteEffects.FlipHorizontally;
                blastCloud.Position = GHelper.Center(spawn, blastCloud.Size) - new Vector2(4, 9);
            }
            else
                blastCloud.Position = GHelper.Center(spawn, blastCloud.Size) - new Vector2(-4, 9);
            blastTimer = (float)blastCloud.AnimationDuration - .1f;
        }

        public void IsHit(int damage)
        {
            HealthPoints -= damage;
            dmgLerp = 0.5f;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);


            if(IsUpgrading)
            {
                var hamSize = new Vector2(100);
                Builder.DrawHammer(sb, new Vector2(Center.X - hamSize.X / 2, CollisionBox.Top - hamSize.Y), hamSize);
            }

            if(DrawTowerInfo || Input.KeyHold(Keys.LeftShift) || isTargeted)
                for(int i = 0; i < stars.Count; i++)
                {
                    var s = stars[i];
                    s.Position = HpBar.Position - new Vector2((i * -s.Size.X) + (i * -6), s.Size.Y + 10);
                    s.Draw(sb);
                }

            if(DrawShootBar)
            {
                if(IsUpgrading || attackTimer < AttackSpeed || isTargeted)
                ShootBar.Draw(sb, new Vector2(GHelper.Center(HpBar.Rectangle, ShootBar.Size).X, HpBar.Rectangle.Top - ShootBar.Size.Y - 4));
            }

            if(Type != TowerType.ConeCatapult)
                if(blastTimer >= 0)
                    blastCloud.Draw(sb);

            DrawShadow(sb);

        }

        public void DrawRange(SpriteBatch sb)
        {
            recf.Draw(sb, Color.LightBlue);
        }

    }
}
