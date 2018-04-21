using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class CompassItem : Sprite
    {

        public Sprite Enemy;

        public CompassItem(EnemyType type) : base(GameContent.compassbox)
        {

            Enemy = new Sprite(GameContent.enemysheet);
            Enemy.SetSize(26);
            Origin = Size * .5f;
        }

        public void Update(GameTime gt, Map map, Enemy en)
        {
            int x = (int)MathHelper.Clamp(Position.X, 0, map.Rectangle.Width - Size.X);
            int y = (int)MathHelper.Clamp(Position.Y, 0, map.Rectangle.Height - Size.Y);

            Position = new Vector2(x, y);

            var dir = en.Center - Position;
            Rotation = (float)Math.Atan2(dir.Y, dir.X);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            Enemy.Draw(sb);
        }

    }

    public class Compass
    {
        public List<CompassItem> Items;

        public Compass()
        {

        }

        public void Add(Enemy en)
        {
            var c = new CompassItem(en.Type);
        }

        public void Update(GameTime gt, Map map, Enemy en)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            foreach(var c in Items)
            {
                c.Draw(sb);
            }
        }

    }
}
