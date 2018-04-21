using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;

namespace ld41gamer.Gamer
{

    public enum BulletType
    {

        Acorn,

        Cone,

    }

    public class Bullet : GameObject
    {
        public float AirVelo;
        public BulletType Type;

        public float LifeTime;

        Vector2 oldPos;

        public Bullet(BulletType t, Vector2 spawnPos, Vector2 destination)
        {
            Type = t;
            Position = spawnPos;
            Direction = Vector2.Normalize(destination - spawnPos);

            SetSize(16);
            SetSourceSize(100);
            SetFrame(0, 0);
            IsAnimating = false;

            Origin = new Vector2(Size.X / 2, Size.Y / 2);

            LifeTime = 5f;

            Texture = GameContent.acorn;

            //Speed = Vector2.Distance(spawnPos, destination) * 2;

            switch(Type)
            {

                case BulletType.Acorn:
                    //Texture = GameContent.acorn;
                    Speed = 800f;
                    AirVelo = -100f;
                    break;

                case BulletType.Cone:
                    break;

            }
        }

        void Gravity(float dt)
        {
            AirVelo += Map.Gravity * dt;
            Position += new Vector2(0, AirVelo * dt);
        }

        public override void UpdatePosition(GameTime gt)
        {
            base.UpdatePosition(gt);

            var dir = Vector2.Normalize(oldPos - Position);
            float angle = (float)Math.Atan2(dir.Y, dir.X) + MathHelper.PiOver2;
            Rotation = angle;
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            var dt = gt.Delta();

            LifeTime -= dt;

            switch(Type)
            {

                case BulletType.Acorn:
                    Gravity(dt);
                    UpdatePosition(gt);
                    break;

                case BulletType.Cone:
                    break;

            }

            oldPos = Position;

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(Texture, Rectangle, )
        }

    }


}
