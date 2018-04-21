using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Gui;
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
        public int HealthPoints { get; set; }

        public List<Rectangle> HitBoxes;

        public List<Rectangle> PlatformCollision;

        public List<TreeBranch> Branches = new List<TreeBranch>();

        public List<Vector2> poss = new List<Vector2>();

        public Tree()
        {
            Texture = GameContent.tree;
            SetSize(Texture.Width, Texture.Height);

            HitBoxes = new List<Rectangle>();
            HitBoxes.Add(new Rectangle(4820, 2480, 420, 150));
            HitBoxes.Add(new Rectangle(4916, 1380, 200, 1200));

            PlatformCollision = new List<Rectangle>();
            //platforms
            PlatformCollision.Add(new Rectangle(5039, 2320, 92, 10));
            PlatformCollision.Add(new Rectangle(4926, 2167, 92, 10));
            PlatformCollision.Add(new Rectangle(5036, 1982, 92, 10));
            PlatformCollision.Add(new Rectangle(4935, 1844, 92, 10));
            PlatformCollision.Add(new Rectangle(5024, 1686, 92, 10));
            PlatformCollision.Add(new Rectangle(4924, 1532, 92, 10));

            //walls


            //branch




            int xStart = 4916;
            int yStart = 1860;
            int xOff = 200;
            int yOff = 300;

            //  bot left

            for(int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 2; x++)
                {
                    poss.Add(new Vector2(xStart + (xOff*x), yStart+(yOff)*y));
                }
            }

            //poss.Add(new Vector2(xStart, yStart));

            //poss.Add(new Vector2(xStart + xOff, yStart));

            //poss.Add(new Vector2(xStart, yStart));

            //poss.Add(new Vector2(xStart, yStart));

            //poss.Add(new Vector2(xStart, yStart));

            //poss.Add(new Vector2(xStart, yStart));

        }

        public void Add(TreeBranchType type, Map map)
        {
            TreeBranch b = new TreeBranch(type);
            b.Position = Position;

            Branches.Add(b);
            Rectangle rec = new Rectangle();

            switch(type)
            {
                case TreeBranchType.TopLeft:
                    rec = new Rectangle(4466, 1550, 454, 15);
                    break;
                case TreeBranchType.TopRight:
                    rec = new Rectangle(5110, 1690, 454, 15);
                    break;
                case TreeBranchType.BotRight:
                    rec = new Rectangle(5125, 1995, 454, 15);
                    break;
                case TreeBranchType.BotLeft:
                    rec = new Rectangle(4454, 1854, 454, 15);
                    break;
            }

            map.CollisionBoxes.Add(new Recc(rec, true));

        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

        }

        public override void Draw(SpriteBatch sb)
        {
            foreach(var b in Branches)
            {
                b.Draw(sb);
            }

            sb.Draw(Texture, Rectangle, Color.White);

        }
    }
}
