using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
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
    public class GameObject : AnimatedSprite
    {

        public Vector2 Center => Position + (Size * .5f);

        public Vector2 Direction;
        public float Speed;

        public virtual void Update(GameTime gt, Map map, GameScreen gs)
        {
            UpdateAnimation(gt);

        }

        public virtual void UpdatePosition(GameTime gt)
        {
            var dt = gt.Delta();
            Position += Speed * Direction * dt;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }

    }
}
