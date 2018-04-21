using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld41gamer.Gamer
{

    public enum BulletType
    {

        Acorn,

        Cone,

    }

    public class Bullet : GameObject
    {

        public BulletType Type;

        public Bullet(BulletType t, Vector2 spawnPos, Vector2 destination)
        {
            Type = t;
            Position = spawnPos;
            Direction = Vector2.Normalize(destination - spawnPos);

            switch(Type)
            {

                case BulletType.Acorn:
                    Speed = 500f;

                    break;

                case BulletType.Cone:

                    break;

            }


        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            switch(Type)
            {

                case BulletType.Acorn:
                    break;

                case BulletType.Cone:
                    break;

            }

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }

    }


}
