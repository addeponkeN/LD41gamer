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
    public enum TreeBranchType
    {
        TopLeft,
        TopRight,
        BotRight,
        BotLeft,
    }

    public class TreeBranch : GameObject
    {
        public TreeBranchType Type;
        public bool Active;

        public TreeBranch(TreeBranchType t)
        {
            Type = t;
            Active = true;
            IsAnimating = false;
            SpriteEffects = SpriteEffects.FlipHorizontally;
            switch(t)
            {

                case TreeBranchType.TopLeft:
                    Texture = GameContent.topleftBranch;
                    break;

                case TreeBranchType.TopRight:
                    Texture = GameContent.toprightBranch;
                    break;

                case TreeBranchType.BotRight:
                    Texture = GameContent.botrightBranch;
                    break;

                case TreeBranchType.BotLeft:
                    Texture = GameContent.botleftBranch;
                    break;

            }

            SetSize(Texture.Width, Texture.Height);
            SetSourceSize((int)Size.X, (int)Size.Y);
            SetFrame(0, 0);
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            if(Active)
                base.Update(gt, map, gs);
        }


        public override void Draw(SpriteBatch sb)
        {
            if(Active)
                base.Draw(sb);
        }
    }
}
