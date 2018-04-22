using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public TreeBranch CurrentBranch;

        public Rectangle BenchRec;

        public Tree()
        {
            Texture = GameContent.tree;

            SetSize(Texture.Width, Texture.Height);
            SetSourceSize(Texture.Width, Texture.Height);
            SetFrame(0, 0);
            IsAnimating = false;

            HitBoxes = new List<Rectangle>();
            HitBoxes.Add(new Rectangle(4820, 2480, 420, 150));
            HitBoxes.Add(new Rectangle(4916, 1180, 200, 1400));

            PlatformCollision = new List<Rectangle>();

            //platforms
            PlatformCollision.Add(new Rectangle(5050, 2320, 72, 10));
            PlatformCollision.Add(new Rectangle(4926, 2167, 92, 10));
            PlatformCollision.Add(new Rectangle(5036, 1988, 92, 10));
            PlatformCollision.Add(new Rectangle(4935, 1852, 92, 10));
            PlatformCollision.Add(new Rectangle(5024, 1686, 92, 10));
            PlatformCollision.Add(new Rectangle(4924, 1542, 92, 10));

            //walls

            BenchRec = new Rectangle(4952, 2470, 100, 100);

        }

        public void Add(TreeBranchType type, Map map)
        {
            TreeBranch b = new TreeBranch(type);
            b.Position = Position;

            Branches.Add(b);
        }

        Rectangle GetRec(TreeBranchType type)
        {
            Rectangle rec = new Rectangle();

            switch(type)
            {
                case TreeBranchType.TopLeft:
                    rec = new Rectangle(4466, 1550, 459, 15);
                    break;
                case TreeBranchType.TopRight:
                    rec = new Rectangle(5110, 1690, 454, 15);
                    break;
                case TreeBranchType.BotRight:
                    rec = new Rectangle(5125, 1995, 454, 15);
                    break;
                case TreeBranchType.BotLeft:
                    rec = new Rectangle(4454, 1854, 477, 15);
                    break;
            }
            return rec;
        }

        public void AddCollisionBranch(TreeBranchType type, Map map)
        {
            var rec = GetRec(type);
            map.CollisionBoxes.Add(new Recc(rec, true));
        }

        public TreeBranchType TYPE(TreeBranchType type)
        {
            if(type == TreeBranchType.BotLeft)
                type = TreeBranchType.BotRight;

            else if(type == TreeBranchType.BotRight)
                type = TreeBranchType.BotLeft;

            else if(type == TreeBranchType.TopLeft)
                type = TreeBranchType.TopRight;

            else
                type = TreeBranchType.TopLeft;

            return type;
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            var p = map.player;

            var mpos = map.MouseWorldPos();

            int ii = -1;
            float near = 99999f;
            for(int i = 0; i < Branches.Count; i++)
            {
                var b = Branches[i];
                b.Update(gt, map, gs);
                if(!b.IsActive)
                {

                    b.Hovered = false;
                    TreeBranchType type = TYPE(b.Type);

                    var rec = GetRec(type);

                    var dis = Vector2.Distance(mpos, rec.Center());

                    if(dis < near)
                    {
                        near = dis;
                        ii = i;
                    }
                }
            }

            if(p.IsBuying)
                if(ii != -1)
                {
                    Branches[ii].Hovered = true;

                    if(Input.LeftClick)
                    {
                        //  buy ~~
                        var type = TYPE(Branches[ii].Type);
                        Branches[ii].IsActive = true;
                        AddCollisionBranch(type, map);
                    }

                }

            if(BenchRec.Intersects(p.CollisionBox))
            {
                if(Input.KeyClick(Keys.F) && p.IsGrounded)
                {
                    p.IsBuying = !p.IsBuying;
                }
            }
            else
                p.IsBuying = false;

        }

        public void Draw(SpriteBatch sb, Map map)
        {
            base.Draw(sb);

            foreach(var b in Branches)
            {
                if(b.IsActive || map.player.IsBuying)
                    b.Draw(sb);
            }
        }
    }
}
