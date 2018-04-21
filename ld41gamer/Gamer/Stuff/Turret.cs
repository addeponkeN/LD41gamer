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

        //  range in pixels
        int Range;

        public Turret(TowerType t)
        {
            Type = t;
            Texture = UtilityContent.box;

            Range = 700;
            attackSpeed = 2;

            switch(t)
            {
                case TowerType.PeaShooter:
                    Name = "Pea Shooter";
                    HealthPoints = 5;
                    Damage = 1;
                    break;
                case TowerType.AcornTurret:
                    Texture = GameContent.turret1;
                    SetSize(130, 74);
                    Name = "Acorn Turret";
                    HealthPoints = 10;
                    Damage = 2;
                    break;
                case TowerType.AcornSniper:
                    Texture = GameContent.turret2;
                    SetSize(130, 93);
                    Name = "Acorn Sniper";
                    HealthPoints = 10;
                    Damage = 3;
                    Range = 1200;
                    break;
                case TowerType.ConeCatapult:
                    Name = "Cone Catapult";
                    HealthPoints = 15;
                    Damage = 3;
                    //SplashDamage = true;
                    break;

            }
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            attackTimer += gt.Delta();

            for(int i = 0; i < map.Enemies.Count; i++)
            {
                var e = map.Enemies[i];
                var distance = Vector2.Distance(Position, e.Position);

                if(distance <= Range)
                    if(attackTimer >= attackSpeed)
                    {
                        Shoot();
                        attackTimer = 0;
                    }
            }
        }

        private void Shoot()
        {
            //  shoot nice acorn
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
