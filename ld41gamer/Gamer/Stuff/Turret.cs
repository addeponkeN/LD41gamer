using ld41gamer.Gamer.Screener;
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

        float attackTimer;
        float attackSpeed;

        public float BuildTime;
        public float BuildTimeBase;

        public bool isBeingBuilt;

        Vector2 bulletStartPosLeft, bulletStartPosRight;



        //  range in pixels
        int Range;

        public Turret(TowerType t)
        {
            Type = t;
            Texture = UtilityContent.box;

            Range = 700;
            attackSpeed = 2;
            IsAnimating = false;

            SetSize(192);
            SetSourceSize(192);

            SetCollisionBot(115, 72);

            Texture = GameContent.turretsheet;

            bulletStartPosRight = new Vector2(156, 135);
            bulletStartPosLeft = new Vector2(27, 135);

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
                    Damage = 2;
                    BuildTimeBase = 4f;
                    SetFrame(0, 0);
                    break;
                case TowerType.AcornSniper:
                    Name = "Acorn Sniper";
                    SetHp(4);
                    Damage = 3;
                    Range = 1200;
                    BuildTimeBase = 7f;
                    SetFrame(0, 1);
                    break;
                case TowerType.ConeCatapult:
                    Name = "Cone Catapult";
                    SetHp(15);
                    Damage = 3;
                    //SplashDamage = true;
                    break;

            }

            //BuildTimeBase = 0.5f;

            CreateBar();
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            attackTimer += gt.Delta();

            for(int i = 0; i < map.Enemies.Count; i++)
            {
                var e = map.Enemies[i];

                bool tryShoot = false;
                //  looking right
                if(SpriteEffects == SpriteEffects.None)
                {
                    if(e.Rectangle.Left > Center.X)
                        tryShoot = true;
                }
                else
                {
                    if(e.Rectangle.Right < Center.X)
                        tryShoot = true;
                }

                if(!tryShoot)
                    continue;

                var distance = Vector2.Distance(Position, e.Position);

                if(distance <= Range)
                    if(attackTimer >= attackSpeed)
                    {
                        if(SpriteEffects == SpriteEffects.None)
                            Shoot(map, Position + bulletStartPosRight, e.Center);
                        else
                            Shoot(map, Position + bulletStartPosLeft, e.Center);

                        attackTimer = 0;
                    }
            }
        }

        private void Shoot(Map map, Vector2 spawn, Vector2 target)
        {
            var dis = Vector2.Distance(spawn, target);
            var off = dis / 10;

            if(dis > 700)
            {
                off += off;
                Console.WriteLine("LONGER RANGER");
            }

            off = Rng.Noxt((int)off - 20, (int)off + 20);
            map.AddBullet(new Bullet(BulletType.Acorn, spawn, target - new Vector2(0, off)));
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
