﻿using ld41gamer.Gamer.Screener;
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

            BenchRec = new Rectangle(4952, 2470, 100, 100);

        }

        public void Add(TreeBranchType type, Map map)
        {
            TreeBranch b = new TreeBranch(type);
            b.Position = Position;

            Branches.Add(b);

            Rectangle rec = GetRec(type);

            map.CollisionBoxes.Add(new Recc(rec, true));

        }

        Rectangle GetRec(TreeBranchType type)
        {
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
            return rec;
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            var p = map.player;

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

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            foreach(var b in Branches)
            {
                b.Draw(sb);
            }
        }
    }
}
