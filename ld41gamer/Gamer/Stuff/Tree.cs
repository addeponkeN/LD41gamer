using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using Obo.Gui;
using Obo.Utility;
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
        public int MaxHealthPoints { get; set; }

        public List<Rectangle> HitBoxes;

        public Rectangle BottomHitBox => HitBoxes[0];

        public List<Rectangle> PlatformCollision;

        public List<TreeBranch> Branches = new List<TreeBranch>();

        public TreeBranch CurrentBranch;

        public Rectangle BenchRec;

        public bool CanBench;

        AnimatedSprite hammer;
        float hammerTimer;
        private float dmgLerp = 1f;

        public Tree()
        {
            Texture = GameContent.tree;
            DrawLayer = Layer.Tree;
            SetSize(Texture.Width, Texture.Height);
            SetSourceSize(Texture.Width, Texture.Height);
            SetFrame(0, 0);
            IsAnimating = false;

            HitBoxes = new List<Rectangle>();
            HitBoxes.Add(new Rectangle(4820, 2480, 420, 150));
            HitBoxes.Add(new Rectangle(4916, 1180, 220, 1400));

            PlatformCollision = new List<Rectangle>();

            //platforms
            PlatformCollision.Add(new Rectangle(5050, 2320, 72, 10));
            PlatformCollision.Add(new Rectangle(4926, 2167, 92, 10));
            PlatformCollision.Add(new Rectangle(5036, 1988, 92, 10));
            PlatformCollision.Add(new Rectangle(4935, 1852, 92, 10));
            PlatformCollision.Add(new Rectangle(5024, 1686, 92, 10));
            PlatformCollision.Add(new Rectangle(4924, 1542, 92, 10));

            BenchRec = new Rectangle(4952, 2470, 100, 100);

            hammer = new AnimatedSprite();
            hammer.SetSize(100);
            hammer.SetSourceSize(100);
            hammer.Texture = GameContent.hammer;
            hammer.PlayAnimation(AnimationType.Hammer);

            HealthPoints = 20;
            MaxHealthPoints = 20;
        }

        public void IsHit(int dmg)
        {
            HealthPoints -= dmg;

            if(HealthPoints < 0)
                HealthPoints = 0;

            dmgLerp = 0.5f;
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

            var dt = gt.Delta();
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

            if(dmgLerp < 1f)
            {
                dmgLerp += dt;
                Color = Color.Lerp(Color.Red, BaseColor, dmgLerp);
            }

            if(Input.RightClick)
                p.IsShoppingBranch = false;

            if(p.IsShopping)
                if(p.IsShoppingBranch)
                    if(ii != -1)
                    {
                        Branches[ii].Hovered = true;
                        if(Input.LeftClick)
                        {
                            if(!map.Game.AnyUiHovered)
                            {
                                if(map.player.Money < TreeBranch.Cost)
                                {
                                    MBMan.Add("Not enough acorns");
                                    return;
                                }
                                else
                                    map.player.Money -= TreeBranch.Cost;
                                //TreeBranch.Cost += 20;

                                var type = TYPE(Branches[ii].Type);
                                Branches[ii].IsActive = true;
                                AddCollisionBranch(type, map);
                                var rec = GetRec(type);
                                for(int i = 0; i < 150; i++)
                                {
                                    var pos = new Vector2(Rng.Noxt(rec.X, rec.Right), Rng.Noxt(rec.Y - 48, rec.Bottom + 92));
                                    Map.pengine.Add(ParticleType.Smoke, pos, Particle.RandomDir());
                                }

                                SoundManager.PlaySound(GameSoundType.TreeBuilding);
                                hammer.Position = new Vector2(GHelper.Center(rec, hammer.Size).X, rec.Top - hammer.Size.Y - 24);
                                hammerTimer = 2f;

                                Upgrades.TreeBranches++;
                            }
                            p.IsShoppingBranch = false;
                        }
                    }

            hammerTimer -= dt;
            if(hammerTimer > 0)
                hammer.UpdateAnimation(gt);
            CanBench = false;
            if(BenchRec.Intersects(p.CollisionBox))
            {
                CanBench = true;
                if(Input.KeyClick(Keys.F) && p.IsGrounded)
                {
                    p.IsShopping = !p.IsShopping;
                }
            }
            else
                p.IsShopping = false;

        }

        public void Draw(SpriteBatch sb, Map map)
        {
            base.Draw(sb);
            if(hammerTimer > 0)
                hammer.Draw(sb);
            foreach(var b in Branches)
            {
                if(b.IsActive || map.player.IsShopping)
                    b.Draw(sb);
            }

            var size = new Vector2(Size.X * 2.5f, 70);

            float x = GHelper.Center(Rectangle, size).X;
            float y = Map.GroundCollisionBox.Top - 28;

            var rec = new Rectangle((int)x, (int)y, (int)size.X, (int)size.Y);
            int alpha = 155;

            sb.Draw(GameContent.shadow, rec, new Color(alpha, alpha, alpha, alpha), Layer.Shadow);
        }
    }
}
