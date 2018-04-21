using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public enum TreeBranchType
    {
        Left,
        Right
    }

    public class TreeBranch : GameObject
    {
        TreeBranchType Type;

        public TreeBranch(TreeBranchType t)
        {
            Type = t;

            switch(t)
            {
                case TreeBranchType.Left:
                    //  Change texture/spriteeffect
                    break;
                case TreeBranchType.Right:
                    break;
            }
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, Color.White);
        }
    }
}
