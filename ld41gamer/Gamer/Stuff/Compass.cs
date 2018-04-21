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


        public CompassItem()
        {

        }

        public void Update(GameTime gt)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }

    }

    public class Compass
    {
        public List<CompassItem> Items;


        public void Draw(SpriteBatch sb)
        {
            
        }
    }
}
