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
        public bool IsActive;
        public bool Hovered;

        public int Cost = 50;

        public TreeBranch(TreeBranchType t)
        {
            Type = t;
            IsActive = false;
            IsAnimating = false;
            SpriteEffects = SpriteEffects.FlipHorizontally;
            DrawLayer = Layer.TreeInside - 0.005f;
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
            if(IsActive)
            {
                Color = new Color(255, 255, 255);
                Alpha = 255;
            }
            else
            {
                int v;

                if(Hovered)
                    v = 150;
                else
                    v = 75;

                Color = new Color(v, v, v);
                Alpha = v;

            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}

