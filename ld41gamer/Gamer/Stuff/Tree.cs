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

        public List<TreeBranch> BranchSlots = new List<TreeBranch>();

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

        public void AddBranch(int slot)
        {

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
