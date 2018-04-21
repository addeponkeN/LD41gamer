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



        public Bullet(BulletType t)
        {
            Type = t;

            switch(Type)
            {
                case BulletType.Acorn:
                    break;
                case BulletType.Cone:
                    break;
            }

        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }

    }


}
