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
    public enum EnemyType
    {
        Ant,
        //Beetle,
        WormBlue,
        WormYellow,
        WormRed,
        Wasp,
        Beaver,

        WormHole,
    }

    public class Enemy : LivingObject
    {
        public EnemyType Type;
        public bool IsFlying;
        public int Reward;

        public float attackCooldown = 2f;
        public float attackTimer;
        public float wormHoleSpawnTimer;

        public bool isAttacking;

        AnimationType animWalk;

        Vector2 ori;

        public float dmgLerp = 1f;

        public Enemy(EnemyType t)
        {
            Type = t;
            Speed = 50f;
            CustomDraw = true;
            DrawLayer = Layer.Enemy;

            switch(t)
            {
                case EnemyType.Ant:
                    Texture = GameContent.antSheet;
                    SetHp(5);
                    Speed = 75f;
                    Damage = 1;
                    Reward = 1;
                    SetSize(165 / 2, 100 / 2);
                    SetCollisionBot(114 / 2, 50 / 2);
                    animWalk = AnimationType.EnemyWalk;
                    break;

                case EnemyType.Wasp:
                    Texture = GameContent.waspSheet;
                    SetSize(152 / 2, 106 / 2);
                    SetCollisionCenter(111 / 2, 84 / 2);
                    animWalk = AnimationType.WaspWalk;
                    SetHp(5);
                    Damage = 1;
                    IsFlying = true;
                    Reward = 3;
                    break;

                case EnemyType.WormHole:
                    Texture = GameContent.wormHole;
                    SetHp(30);
                    SetSize(Texture.Width / 2, Texture.Height / 2);
                    Speed = 0;
                    Damage = 0;
                    Reward = 10;
                    break;

                case EnemyType.WormBlue:
                    Texture = GameContent.wormSheet;
                    SetHp(5);
                    Damage = 1;
                    Reward = 3;
                    SetSize(170 / 2, 100 / 2);
                    SetCollisionBot(105 / 2, 23 / 2);
                    animWalk = AnimationType.WormBlue;
                    break;

                case EnemyType.WormYellow:
                    Texture = GameContent.wormSheet;
                    SetHp(3);
                    Damage = 1;
                    Reward = 1;
                    SetSize(170 / 2, 100 / 2);
                    SetCollisionBot(105 / 2, 23 / 2);
                    animWalk = AnimationType.WormYellow;

                    break;

                case EnemyType.WormRed:
                    Texture = GameContent.wormSheet;
                    SetHp(7);
                    Damage = 2;
                    Reward = 5;
                    SetSize(170 / 2, 100 / 2);
                    SetCollisionBot(105 / 2, 23 / 2);
                    animWalk = AnimationType.WormRed;

                    break;

                case EnemyType.Beaver:
                    Texture = GameContent.beaverSheet;
                    SetHp(30);
                    Damage = 2;
                    Reward = 25;
                    SetSize(95, 77);
                    SetCollisionBot(80, 75);
                    animWalk = AnimationType.BeaverWalk;
                    break;

            }

            PlayAnimation(animWalk);

            CreateBar();
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            var dt = gt.Delta();

            if(dmgLerp < 1f)
            {
                dmgLerp += dt;
                Color = Color.Lerp(Color.Red, BaseColor, dmgLerp);
            }


            if(Type == EnemyType.WormHole)
            {
                wormHoleSpawnTimer += dt;
                if(wormHoleSpawnTimer >= 5)
                {
                    map.SpawnWorm((int)Position.X);
                    wormHoleSpawnTimer = 0;
                }
            }

            foreach(var t in map.tree.HitBoxes)
            {
                if(Rectangle.Intersects(t))
                {
                    attackTimer += dt;
                    if(attackTimer >= attackCooldown)
                    {
                        map.tree.HealthPoints -= Damage;
                        attackTimer = 0;
                    }
                    return;
                }
            }

            //foreach(var t in map.Turrets)
            //{
            //    if(Rectangle.Intersects(t.CollisionBox))
            //    {
            //        attackTimer += dt;
            //        if(attackTimer >= 2)
            //        {
            //            t.HealthPoints -= Damage;
            //            attackTimer = 0;
            //        }
            //        return;
            //    }
            //}

            if(!isAttacking)
            {
                ori = Vector2.Zero;
                IsAnimating = true;
                PlayAnimation(animWalk);
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
            else
            {
                IsAnimating = false;
                CurrentAnimationFrame = CurrentAnimation[0];

                ori.X = MathHelper.Lerp(0, 16, attackTimer / attackCooldown);
            }

        }

        public static EnemyType RandomType()
        {
            return (EnemyType)Rng.Noxt(0, Enum.GetValues(typeof(EnemyType)).Length - 2);
        }

        public static EnemyType RandomTypeNotWormHole()
        {
            return (EnemyType)Rng.Noxt(0, Enum.GetValues(typeof(EnemyType)).Length - 2);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, new Rectangle((int)(Position.X + ori.X), (int)(Position.Y + ori.Y), Rectangle.Width, Rectangle.Height), CurrentAnimationFrame, new Color(Color, Alpha), Rotation, Origin, SpriteEffects, DrawLayer);

            base.Draw(sb);


            var size = new Vector2(CollisionBox.Width, 6 + (CollisionBox.Height / 3));

            float x = GHelper.Center(CollisionBox, size).X;
            float y;

            if(CollisionBox.Bottom < Map.GroundCollisionBox.Top)
            {
                y = Map.GroundCollisionBox.Top - size.Y;
            }
            else y = CollisionBox.Bottom - size.Y;

            var pos = new Vector2(x, y);

            DrawShadow(sb);
        }

    }
}
