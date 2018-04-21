using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer.Stuff
{
    public class Layer
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Size;
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Globals.Scale.X), (int)(Size.Y * Globals.Scale.Y));
        
        public Layer()
        {

        }


        public void Update(Map map)
        {

            


        }

    }
}
