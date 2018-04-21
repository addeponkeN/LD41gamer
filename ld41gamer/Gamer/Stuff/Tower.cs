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
        ConeCatapult
    }

    public class Tower : LivingObject
    {
        TowerType Type;

        public Tower(TowerType t)
        {
            Type = t;
            Texture = UtilityContent.box;

            switch(t)
            {
                case TowerType.PeaShooter:
                    Name = "Pea Shooter";
                    HealthPoints = 5;
                    Damage = 1;
                    break;
                case TowerType.AcornTurret:
                    Name = "Acorn Turret";
                    HealthPoints = 10;
                    Damage = 2;
                    break;
                case TowerType.ConeCatapult:
                    Name = "Cone Catapult";
                    HealthPoints = 15;
                    Damage = 3;
                    //SplashDamage = true;
                    break;
            }
        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, Color.White);
        }
    }
}
