using ld41gamer.Gamer.Screener;
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
    public class Player : GameObject
    {

        public float JumpPower = 775;
        public bool IsGrounded;
        public float JumpVelo;

        public bool IsFalling => JumpVelo > -0.1f;

        public float ShootCooldownTimer;

        public bool IsMoving { get; set; }

        public int Money;

        public Player()
        {
            Texture = GameContent.playerSheet;
            Speed = 290;
            Size = new Vector2(165, 100);
            SetCollisionBot(40, 80);

            PlayAnimation(AnimationType.Idle);
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            var dt = gt.Delta();
            //UpdatePosition(gt);

            Position += new Vector2(Direction.X * Speed * dt, JumpVelo * dt);

            if(!IsGrounded)
                JumpVelo += Map.Gravity * dt;

            if(Input.KeyHold(Keys.LeftShift))
                Speed = 1200;
            else
                Speed = 290;

            Direction.X = 0;
            //Direction.Y = 0;

            if(Input.KeyHold(Keys.A))
            {
                Run(-1);
                PlayAnimation(AnimationType.PlayerWalking);
            }

            if(Input.KeyHold(Keys.D))
            {
                Run(1);
                PlayAnimation(AnimationType.PlayerWalking);
            }

            if(Input.KeyClick(Keys.Space))
            {
                if(IsGrounded)
                    Jump();
            }

            if(Direction.X == 0)
            {
                IsMoving = false;
                PlayAnimation(AnimationType.Idle);
            }


            if(ShootCooldownTimer < 0)
            {
                if(Input.LeftHold)
                {
                    Shoot(map);
                    ShootCooldownTimer = 0.75f;
                }
            }
            else
            {
                ShootCooldownTimer -= dt;
            }
        }


        void Jump()
        {
            IsGrounded = false;
            JumpVelo = -JumpPower;
        }

        void Run(int dir)
        {
            IsMoving = true;
            Direction.X = dir;

            if(dir == -1)
            {
                SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
                SpriteEffects = SpriteEffects.None;
        }


        void Shoot(Map map)
        {
            var des = map.MouseWorldPos();

            var b = new Bullet(BulletType.Acorn, Center, des);

            map.AddBullet(b);
        }


        public void Collision(List<Recc> recs)
        {


            IsGrounded = false;
            foreach(var recc in recs)
            {
                var rec = recc.Rec;



                if(recc.IsPlatform)
                {
                    if(Input.KeyHold(Keys.S))
                        continue;

                    if(rec.Bottom < Rectangle.Bottom)
                        continue;

                    if(IsFalling)
                    {
                        if(rec.Intersects(CollisionBox))
                        {
                            if(CollisionBox.Bottom >= rec.Y + 1)
                            {
                                Position = new Vector2(Position.X, rec.Y - Size.Y + 1);
                                IsGrounded = true;
                            }
                        }
                    }

                }
                else
                {
                    if(JumpVelo > -1f)
                        if(CollisionBox.Bottom >= rec.Top)
                        {
                            Position = new Vector2(Position.X, rec.Y - Size.Y);
                            IsGrounded = true;
                        }

                    if(rec.TouchLeftOf(rec))
                    {
                        Position = new Vector2(rec.X - Size.X, Position.Y);
                        IsGrounded = true;
                    }

                    if(rec.TouchRightOf(rec))
                    {
                        Position = new Vector2(rec.X, Position.Y);
                        IsGrounded = true;
                    }

                }


            }

            if(IsGrounded)
            {
                JumpVelo = 0f;
            }

        }

        public void PlatformCollision(Rectangle rec)
        {

            //if(!Input.KeyHold(Keys.S) && Rectangle.Bottom < rec.Top + 4)
            //{
            //    IsGrounded = false;
            //    JumpVelo = 0;
            //    Position = new Vector2(Position.X, rec.Y - Size.Y);
            //}

            //if(Rectangle.Bottom > rec.Top)
            //{
            //    IsJumping = false;
            //    JumpVelo = 0;
            //    Position = new Vector2(Position.X, rec.Y - Size.Y);

            //}

            //if(Rectangle.Bottom > rec.Top)
            //{
            //    IsJumping = false;
            //    JumpVelo = 0;
            //    Position = new Vector2(Position.X, rec.Y - Size.Y);

            //}

            //if(Rectangle.Bottom > rec.Top)
            //{
            //    IsJumping = false;
            //    JumpVelo = 0;
            //    Position = new Vector2(Position.X, rec.Y - Size.Y);

            //}




            //if(rec.TouchTopOf(rec))
            //{
            //    Position = new Vector2(Position.X, rec.Y - Size.Y);
            //    JumpVelo = 0;
            //}

            //if(rec.TouchBottomOf(rec))
            //{
            //    Position = new Vector2(Position.X, rec.Y);
            //    JumpVelo = 0;
            //}

            //if(rec.TouchLeftOf(rec))
            //{
            //    Position = new Vector2(rec.X - Size.X, Position.Y);
            //    JumpVelo = 0;
            //}

            //if(rec.TouchRightOf(rec))
            //{
            //    Position = new Vector2(rec.X, Position.Y);
            //    JumpVelo = 0;
            //}
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(Texture, , Color.White);

            if(Globals.IsDebugging)
            sb.Draw(UtilityContent.box, CollisionBox, Color.Blue);
        }

    }
}
