using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using Obo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class Builder
    {
        public List<Turret> Con;

        public static bool IsPlacing;

        static AnimatedSprite hammer;
        static Sprite ug;
        static Sprite rep;

        public Turret b;

        float pTimer;

        public Builder()
        {
            Con = new List<Turret>();

            hammer = new AnimatedSprite();
            hammer.SetSize(100);
            hammer.SetSourceSize(100);
            hammer.Texture = GameContent.hammer;
            hammer.PlayAnimation(AnimationType.Hammer);

            ug = new Sprite(GameContent.icons);
            ug.SetSize(64);
            ug.SetSourceSize(64);
            ug.SetFrame(1, 0);

            rep = new Sprite(GameContent.icons);
            rep.SetSize(64);
            rep.SetSourceSize(64);
            rep.SetFrame(2, 0);
        }

        public void Update(GameTime gt, Map map)
        {
            var dt = gt.Delta();
            var p = map.player;

            bool canPlace = false;

            if(b != null)
            {
                if(p.IsGrounded)
                {
                    if(map.tree.HitBoxes.Any(x => x.Intersects(b.CollisionBox)))
                    {
                        canPlace = false;
                    }
                    else if(map.Turrets.Any(x => x.CollisionBox.Intersects(b.CollisionBox)))
                    {
                        canPlace = false;
                    }
                    else if(b.Rectangle.Bottom < 2100)
                    {
                        if(map.CollisionBoxes.Any(x => x.Rec.Intersects(b.CollisionBox)))
                        {
                            canPlace = true;
                        }
                        else
                            canPlace = false;
                    }
                    else if(b.Rectangle.Bottom > 2450)
                        canPlace = true;
                }
            }

            map.player.IsBuilding = false;

            for(int i = 0; i < Con.Count; i++)
            {
                var t = Con[i];
                t.HpBar.Update(t.BuildTime, t.BuildTimeBase);
                t.isBeingBuilt = false;

                if(canPlace)
                    if(b != null)
                    {
                        canPlace = !t.CollisionBox.Intersects(b.CollisionBox);
                    }

                if(map.player.CollisionBox.Intersects(t.CollisionBox))
                {
                    if(Input.KeyHold(Keys.F))
                    {
                        pTimer += dt;
                        if(pTimer > 0.4)
                        {
                            //var pos = new Vector2(p.CollisionBox.Center().X + Rng.Noxt(-48, 48), p.CollisionBox.Center().Y + Rng.Noxt(-48, 48));
                            var pos = t.CollisionBox.Center() + new Vector2(36, 28) + new Vector2(Rng.Noxt(-48, 48), Rng.Noxt(-48, 48));
                            Map.pengine.Add(ParticleType.Smoke, pos, Particle.RandomDir());
                            pTimer = 0;
                        }
                        //  is building
                        t.BuildTime += dt * p.BuildSpeed;
                        map.player.IsBuilding = true;
                        t.isBeingBuilt = true;
                        if(t.BuildTime >= t.BuildTimeBase)
                        {
                            //  build compelte
                            var turr = new Turret(t.Type);

                            turr.Position = t.Position;
                            turr.SetEffect(t.SpriteEffects);
                            turr.DrawShootBar = true;
                            map.Turrets.Add(turr);
                            Con.RemoveAt(i);
                        }
                        break;
                    }
                }
            }


            if(Input.KeyClick(Keys.D1))
            {
                b = Create(TowerType.AcornTurret);
            }
            if(Input.KeyClick(Keys.D2))
            {
                b = Create(TowerType.AcornSniper);
            }
            if(Input.KeyClick(Keys.D3))
            {
                b = Create(TowerType.ConeCatapult);
            }
            if(Input.KeyClick(Keys.D4))
            {

            }

            IsPlacing = false;
            if(b != null)
            {
                b.UpdateRec();
                IsPlacing = true;

                Vector2 pos;
                if(map.player.LatestDirection == 1)
                {
                    pos = new Vector2(p.CollisionBox.Right, p.CollisionBox.Bottom - b.Size.Y);
                }
                else
                {
                    pos = new Vector2(p.CollisionBox.Left - b.Size.X, p.CollisionBox.Bottom - b.Size.Y);
                }

                b.Position = pos;
                b.SpriteEffects = p.SpriteEffects;

                if(canPlace)
                {
                    b.Color = Color.Lerp(Color.White, Color.ForestGreen, 0.5f);

                    if(Input.LeftClick)
                    {
                        if(map.player.Money < b.Cost)
                        {
                            Console.WriteLine("NO AFFORD");
                            return;
                        }
                        else
                            map.player.Money -= b.Cost;

                        var t = new Turret(b.Type) { Position = b.Position, };
                        t.Column = 3;
                        t.SetFrame(t.Column, t.Row);
                        t.Color = Color.CornflowerBlue;
                        t.SetEffect(b.SpriteEffects);
                        t.HpBar.Foreground.Color = Color.LightGoldenrodYellow;
                        t.HpBar.Background.Color = Color.DarkGoldenrod;

                        Con.Add(t);
                        b = null;
                        p.ShootCooldownTimer = p.ShootCooldown;
                    }

                }
                else
                {
                    b.Color = Color.Lerp(Color.White, Color.IndianRed, 0.5f);
                }


                if(Input.RightClick)
                    b = null;
            }

            hammer.UpdateAnimation(gt);

        }

        Turret Create(TowerType type)
        {
            var t = new Turret(type);
            t.DrawHpBar = false;
            t.Color = new Color(127, 127, 127);
            t.Alpha = 127;
            return t;
        }

        public static void DrawHammer(SpriteBatch sb, Vector2 position, Vector2 size)
        {
            hammer.Position = position;
            hammer.Draw(sb);
        }

        public static void DrawUpgradeAndRepair(SpriteBatch sb, Vector2 pos, Vector2 size)
        {
            ug.Position = pos;
            ug.Size = size;

            rep.Position = pos + new Vector2(ug.Size.X + 12, 0);
            rep.Size = size;

            ug.Draw(sb);
            rep.Draw(sb);

            var s = GameContent.font14.MeasureString("[R]");

            Extras.DrawString(sb, GameContent.font14, "[G]", new Vector2(GHelper.Center(ug.Rectangle, s).X, ug.Position.Y - 15), Color.White);
            Extras.DrawString(sb, GameContent.font14, "[R]", new Vector2(GHelper.Center(rep.Rectangle, s).X, rep.Position.Y - 15), Color.White);

        }

        public static void DrawBenchUpgrade(SpriteBatch sb, Vector2 pos, Vector2 size)
        {
            ug.Position = pos;
            ug.Size = size;

            ug.Draw(sb);
            var s = GameContent.font14.MeasureString("[F]");
            Extras.DrawString(sb, GameContent.font14, "[F]", new Vector2(GHelper.Center(ug.Rectangle, s).X, ug.Position.Y - 15), Color.White);
        }

        public void Draw(SpriteBatch sb)
        {

            foreach(var t in Con)
            {
                t.Draw(sb);

                if(t.isBeingBuilt)
                {
                    var hamSize = new Vector2(100);
                    DrawHammer(sb, new Vector2(t.Center.X - hamSize.X / 2, t.CollisionBox.Top - hamSize.Y), hamSize);
                }
            }

            b?.Draw(sb);


        }

        public void DrawRecs(SpriteBatch sb)
        {
            b?.DrawRange(sb);
        }
    }
}
