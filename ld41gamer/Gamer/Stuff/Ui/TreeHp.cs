using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;

namespace ld41gamer.Gamer
{
    public class TreeHp : HpBar
    {

        public Sprite Tree;

        public TreeHp() : base(300, 40)
        {
            Tree = new Sprite(GameContent.tree);
            Tree.SetSize(96, 128);
        }

        public void Posser(Vector2 pos)
        {
            Tree.Position = pos;
            Position = new Vector2(Tree.Rectangle.Right + 2, GHelper.Center(Tree.Rectangle, Size).Y);
        }

        public override void Draw(SpriteBatch sb)
        {
            Draw(sb, Position);
            Tree.Draw(sb);

        }

    }
}
