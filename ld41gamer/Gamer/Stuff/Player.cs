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

        public float JumpPower = 300;
        public float JumpVelo;
        public bool IsJumping = true;

        public float ShootCooldownTimer;

        public Player()
        {
            Texture = GameContent.playerSheet;
            Speed = 200;
            Size = new Vector2(165, 100);

            PlayAnimation(AnimationType.PlayerIdle);
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            var dt = gt.Delta();
            //UpdatePosition(gt);

            Position += new Vector2(Direction.X * Speed * dt, JumpVelo * dt);

            if(IsJumping)
                JumpVelo += Map.Gravity * dt;


            if(Input.KeyHold(Keys.LeftShift))
                Speed = 800;
            else
                Speed = 200;

            Direction = Vector2.Zero;
            if(Input.KeyHold(Keys.W))
            {
                Direction.Y = -1;
                PlayAnimation(AnimationType.PlayerWalking);
            }

            if(Input.KeyHold(Keys.S))
            {
                Direction.Y = 1;
                PlayAnimation(AnimationType.PlayerWalking);
            }

            if(Input.KeyHold(Keys.A))
            {
                Direction.X = -1;
                PlayAnimation(AnimationType.PlayerWalking);
            }

            if(Input.KeyHold(Keys.D))
            {
                Direction.X = 1;
                PlayAnimation(AnimationType.PlayerWalking);
            }

            if(Input.KeyClick(Keys.Space))
            {
                //if(!IsJumping)
                    Jump();
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
            IsJumping = true;
            JumpVelo = -JumpPower;
        }


        void Shoot(Map map)
        {
            var des = map.MouseWorldPos();

            var b = new Bullet(BulletType.Acorn, Center, des);

            map.AddBullet(b);
        }


        public void Collision(Rectangle rec)
        {

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


            if(Rectangle.Bottom > rec.Top)
            {
                IsJumping = false;
                JumpVelo = 0;
                Position = new Vector2(Position.X, rec.Y - Size.Y);
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(Texture, , Color.White);

        }

    }
}
