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
        public int Reward;

        float attackTimer;

        public Enemy(EnemyType t)
        {
            Type = t;
            Speed = 50f;
            SetSize(165 / 2, 100 / 2);

            PlayAnimation(AnimationType.EnemyWalk);

            switch(t)
            {
                case EnemyType.Ant:
                    Texture = GameContent.antSheet;
                    HealthPoints = 3;
                    Damage = 1;
                    Reward = 1;
                    break;
                case EnemyType.Beetle:
                    HealthPoints = 7;
                    Damage = 2;
                    Reward = 2;
                    break;
                case EnemyType.FlyingAnt:
                    HealthPoints = 3;
                    Damage = 1;
                    IsFlying = true;
                    Reward = 3;
                    break;
            }
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            var dt = gt.Delta();

            foreach(var t in map.tree.HitBoxes)
            {
                if(Rectangle.Intersects(t))
                {
                    attackTimer += dt;
                    if(attackTimer >= 2)
                    {
                        map.tree.HealthPoints -= Damage;
                        attackTimer = 0;
                    }
                    return;
                }
            }
            foreach(var t in map.Turrets)
            {
                if(Rectangle.Intersects(t.Rectangle))
                {
                    attackTimer += dt;
                    if(attackTimer >= 2)
                    {
                        t.HealthPoints -= Damage;
                        attackTimer = 0;
                    }
                    return;
                }
            }

            Position += new Vector2(Direction.X * Speed * dt, 0);

            if(Position.X > map.GroundRectangle.Center.X)
            {
                Direction.X = -1;
                SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                Direction.X = 1;
            }
        }


        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(Texture, Rectangle, Color.White);

        }
    }
}
