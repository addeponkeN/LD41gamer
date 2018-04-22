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
        public float BulletDrop;

        Vector2 oldPos;

        public int Damage = 1;

        public Bullet(BulletType t, Vector2 spawnPos, Vector2 destination, int damage, bool isPlayer = false)
        {
            Type = t;
            Position = spawnPos;
            Direction = Vector2.Normalize(destination - spawnPos);
            Damage = damage;

            if(isPlayer)
                SetSize(16);
            else
                SetSize(28);
            SetSourceSize(100);
            SetFrame(0, 0);
            IsAnimating = false;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            LifeTime = 20f;

            DrawLayer = Layer.Bullet;

            switch(Type)
            {

                case BulletType.Acorn:
                    if(Speed == 0)
                        Speed = 600f;
                    AirVelo = -45;
                    Texture = GameContent.acorn;
                    break;

                case BulletType.Cone:

                    var dis = Vector2.Distance(spawnPos, destination);
                    Speed = 200f + (dis * .35f);
                    AirVelo = -10f;
                    BulletDrop = 50f + (dis*0.1f);
                    Texture = GameContent.cone;
                    break;

            }

            if(isPlayer)
            {
                BulletDrop = 650f;
                Speed += 120;
            }




        }

        void Gravity(float dt)
        {
            AirVelo += BulletDrop * dt;
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
                    Gravity(dt);
                    UpdatePosition(gt);
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
