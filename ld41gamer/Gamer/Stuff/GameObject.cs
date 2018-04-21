using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer.Stuff
{
    public class GameObject : AnimatedSprite
    {

        public float Speed;

        public void Update(GameTime gt, GameScreen gs)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }

    }
}
