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
    public enum EnemyType
    {
        Ant,
        Beetle,
        FlyingAnt
    }

    public class Enemy : LivingObject
    {
        public EnemyType Type;
        public bool IsFlying;

        public Enemy(EnemyType t)
        {
            Type = t;
            Speed = 50f;

            switch(t)
            {
                case EnemyType.Ant:
                    HealthPoints = 3;
                    Damage = 1;
                    break;
                case EnemyType.Beetle:
                    HealthPoints = 7;
                    Damage = 2;
                    break;
                case EnemyType.FlyingAnt:
                    HealthPoints = 3;
                    Damage = 1;
                    IsFlying = true;
                    break;
            }
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            var dt = gt.Delta();

            Position += new Vector2(Direction.X * Speed * dt, 0);

            if(Position.X > map.GroundRectangle.Center.X)
            {
                Direction.X = -1;
            }
            else
            {
                Direction.X = 1;
            }
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(UtilityContent.box, Rectangle, Color.Red);

        }
    }
}
