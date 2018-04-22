using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
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

        AnimatedSprite hammer;

        public Turret b;

        public Builder()
        {
            Con = new List<Turret>();

            hammer = new AnimatedSprite();
            hammer.SetSize(100);
            hammer.SetSourceSize(100);
            hammer.Texture = GameContent.hammer;
            hammer.PlayAnimation(AnimationType.Hammer);
        }

        public void Add()
        {

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
                        //  is building
                        t.BuildTime += dt;
                        map.player.IsBuilding = true;
                        t.isBeingBuilt = true;
                        if(t.BuildTime >= t.BuildTimeBase)
                        {
                            //  build compelte
                            var turr = new Turret(t.Type);
                            turr.Position = t.Position;
                            turr.SpriteEffects = t.SpriteEffects;
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

            }
            if(Input.KeyClick(Keys.D4))
            {

            }

            IsPlacing = false;
            if(b != null)
            {
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
                        var t = new Turret(b.Type) { Position = b.Position, };
                        t.Column++;
                        t.SetFrame(t.Column, t.Row);
                        t.Color = Color.CornflowerBlue;
                        t.SpriteEffects = b.SpriteEffects;
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

        public void Draw(SpriteBatch sb)
        {

            foreach(var t in Con)
            {
                t.Draw(sb);

                if(t.isBeingBuilt)
                {
                    hammer.Position = new Vector2(t.Center.X - hammer.Size.X / 2, t.CollisionBox.Top - hammer.Size.Y);
                    hammer.Draw(sb);
                }
            }

            b?.Draw(sb);

        }
    }
}
