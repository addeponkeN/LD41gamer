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

        public Bullet(BulletType t, Vector2 spawnPos, Vector2 destination)
        {
            Type = t;
            Position = spawnPos;
            Direction = Vector2.Normalize(destination - spawnPos);

            SetSize(16);
            SetFrame(0, 0);
            IsAnimating = false;

            LifeTime = 5f;

            //Speed = Vector2.Distance(spawnPos, destination) * 2;

            switch(Type)
            {

                case BulletType.Acorn:
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

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(Texture, Rectangle, )
        }

    }


}
