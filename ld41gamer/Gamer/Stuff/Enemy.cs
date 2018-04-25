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
        WormYellow,
        WormBlue,
        Wasp,
        Ant,
        WormRed,
        WormHole,
        Beaver,
        //Beetle,
    }

    public class Enemy : LivingObject
    {
        public EnemyType Type;
        public bool IsFlying;
        public int Reward;

        public float attackCooldown = 2f;
        public float attackTimer;
        public float wormHoleSpawnTimer;
        public float wormHoleSpawnCd = 6.5f;

        public bool isAttacking;
        public bool treeattack;

        AnimationType animWalk;

        Vector2 ori;

        public float dmgLerp = 1f;

        public static float HpIncreaser = 0;

        public Enemy(EnemyType t)
        {
            Type = t;
            Speed = 50f;
            CustomDraw = true;
            DrawLayer = Layer.Enemy;

            switch(t)
            {
                case EnemyType.WormYellow:
                    Texture = GameContent.wormSheet;
                    SetHp(2);
                    Damage = 1;
                    Reward = 2;
                    SetSize(170 / 2, 100 / 2);
                    SetCollisionBot(105 / 2, 23 / 2);
                    animWalk = AnimationType.WormYellow;
                    SetFrame(1, 0);
                    break;

                case EnemyType.WormBlue:
                    Texture = GameContent.wormSheet;
                    SetHp(5);
                    Damage = 1;
                    Reward = 3;
                    SetSize(170 / 2, 100 / 2);
                    SetCollisionBot(105 / 2, 23 / 2);
                    animWalk = AnimationType.WormBlue;
                    SetFrame(2, 0);
                    break;

                case EnemyType.Wasp:
                    Texture = GameContent.waspSheet;
                    SetSize(152 / 2, 106 / 2);
                    SetCollisionCenter(111 / 2, 84 / 2);
                    animWalk = AnimationType.WaspWalk;
                    SetHp(4);
                    Damage = 1;
                    IsFlying = true;
                    Reward = 4;
                    break;

                case EnemyType.Ant:
                    Texture = GameContent.antSheet;
                    SetHp(6);
                    Speed = 100f;
                    Damage = 1;
                    Reward = 4;
                    SetSize(165 / 2, 100 / 2);
                    SetCollisionBot(114 / 2, 50 / 2);
                    animWalk = AnimationType.EnemyWalk;
                    break;

                case EnemyType.WormRed:
                    Texture = GameContent.wormSheet;
                    SetHp(8);
                    Damage = 1;
                    Reward = 4;
                    SetSize(170 / 2, 100 / 2);
                    SetCollisionBot(105 / 2, 23 / 2);
                    animWalk = AnimationType.WormRed;
                    SetFrame(0, 0);
                    break;

                case EnemyType.WormHole:
                    Texture = GameContent.wormHole;
                    SetHp(20);
                    SetSize(Texture.Width, Texture.Height);
                    SetCollisionBot(108, 21);
                    Speed = 0;
                    Damage = 0;
                    Reward = 6;
                    animWalk = AnimationType.WormHole;
                    IsAnimating = false;
                    break;

                case EnemyType.Beaver:
                    Texture = GameContent.beaverSheet;
                    SetHp(30);
                    Damage = 1;
                    Reward = 8;
                    SetSize(95, 77);
                    SetCollisionBot(80, 75);
                    animWalk = AnimationType.BeaverWalk;
                    break;
            }

            HealthPoints += (int)HpIncreaser;
            MaxHealthPoints = HealthPoints;

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
                if(wormHoleSpawnTimer >= wormHoleSpawnCd)
                {
                    map.SpawnWorm((int)Position.X);
                    wormHoleSpawnTimer = 0;
                }
                return;
            }

            bool att = false;

            foreach(var t in map.tree.HitBoxes)
            {
                if(Rectangle.Intersects(t))
                {
                    attackTimer += dt;
                    if(attackTimer >= attackCooldown)
                    {
                        map.tree.IsHit(Damage);
                        attackTimer = 0;
                        SoundManager.PlayTowerHit();
                    }
                    att = true;
                    //return;
                }
            }

            treeattack = att;
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

            if(!isAttacking && !treeattack)
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

                if(SpriteEffects == SpriteEffects.None)
                    ori.X = -MathHelper.Lerp(0, 16, attackTimer / attackCooldown);
                else
                    ori.X = MathHelper.Lerp(0, 16, attackTimer / attackCooldown);
            }

        }

        public static EnemyType GetRandomType()
        {
            return (EnemyType)Rng.Noxt(0, Enum.GetValues(typeof(EnemyType)).Length - 1);
        }
        public static EnemyType GetRandomType(params EnemyType[] types)
        {
            return types[Rng.Next(0, types.Length - 1)];
        }

        public static EnemyType GetRandomTypeExcept(params EnemyType[] exceptions)
        {
            var list = Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().ToList();
            for(int i = 0; i < list.Count; i++)
            {
                if(exceptions.Any(x => x == list[i]))
                {
                    list.RemoveAt(i);
                    if(i > 0)
                        i--;
                }
            }
            return list[Rng.Next(0, list.Count - 1)];
        }

        public static EnemyType RandomTypeNotWormHole()
        {
            int e = Rng.Noxt(0, Enum.GetValues(typeof(EnemyType)).Length - 1);

            while((EnemyType)e == EnemyType.WormHole)
                e = Rng.Noxt(0, Enum.GetValues(typeof(EnemyType)).Length - 1);

            return (EnemyType)e;
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
