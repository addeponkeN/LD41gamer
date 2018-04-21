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
    public class Enemy : LivingObject
    {

        public Enemy()
        {

        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(UtilityContent.box, Rectangle, Color.Red);

        }
    }
}
