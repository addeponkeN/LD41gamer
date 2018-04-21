using ld41gamer.Gamer.Screener;
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
    public class Tree : GameObject
    {
        public int Level { get; set; }

        public Tree()
        {
            Texture = GameContent.tree; 
        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, Color.White);

        }
    }
}
