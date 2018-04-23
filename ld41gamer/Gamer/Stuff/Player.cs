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
    public class Player : LivingObject
    {

        public float JumpPower = 775;
        public bool IsGrounded;
        public float JumpVelo;

        public bool IsFalling => JumpVelo > -0.1f;

        public float ShootCooldownTimer;
        public float ShootCooldown = 0.80f;

        public bool IsMoving { get; set; }
        public bool IsBuilding { get; set; }
        public bool IsShoppingBranch { get; internal set; }
        public bool IsUpgradingOrReparing { get; internal set; }

        public bool CanMove => !IsBuilding && !IsUpgradingOrReparing;

        public bool IsShopping;

        public int LatestDirection;

        public float BuildSpeed = 1f;

        public float deadTimer;
        public float respawnTimer;

        public bool isRespawning;
        public bool respawned;

        bool toBlack;

        public float knockSpeed;

        int money = 500;
        //public int Money = 500;
        public int Money
        {
            get => money; set
            {
                int dif = money - value;

                if(value > money)
                    MBMan.Add("+" + -dif);
                else
                    MBMan.Add("-" + dif);

                money = value;
            }
        }

        public float dmgLerp = 1f;

        public Player()
        {
            Texture = GameContent.playerSheet;
            Speed = 290;
            Size = new Vector2(165, 100);
            SetCollisionBot(40, 80);

            SetHp(1);

            PlayAnimation(AnimationType.Idle);
            SetFrame(0, 0);

            DrawLayer = Layer.Player;

            //CreateBar();
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            var dt = gt.Delta();
            //UpdatePosition(gt);

            if(dmgLerp < 1f)
            {
                dmgLerp += dt;
                Color = Color.Lerp(Color.Red, BaseColor, dmgLerp);
            }

            if(IsAlive)
            {
                Position += new Vector2(Direction.X * Speed * dt, JumpVelo * dt);

                if(!IsGrounded)
                    JumpVelo += Map.Gravity * dt;

                if(Input.KeyHold(Keys.LeftShift))
                    Speed = 1200;
                else
                    Speed = 290;

                Direction.X = 0;
                //Direction.Y = 0;

                if(CanMove)
                {

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
                }

                if(Direction.X == 0)
                {
                    IsMoving = false;
                }

                if(!IsGrounded && !IsFalling)
                    PlayAnimation(AnimationType.PlayerJumping);
                else if(JumpVelo > 25)
                    PlayAnimation(AnimationType.PlayerFalling);
                else if(Direction.X == 0 && IsGrounded)
                    PlayAnimation(AnimationType.Idle);

                if(Direction.X != 0)
                    LatestDirection = (int)Direction.X;
            }
            else
            {
                //  is dead

                if(frame >= CurrentAnimation.Length - 2)
                    frame = CurrentAnimation.Length - 2;

                //  dead for abit,  
                deadTimer -= dt;
                knockSpeed -= dt * 800f;
                if(knockSpeed <= 1)
                    knockSpeed = 0f;

                Position.X += Direction.X * knockSpeed * dt;


                //  dead complete, start respawn


                //  start fade to black state
                if(deadTimer <= 0 && !isRespawning)
                {
                    isRespawning = true;
                    respawnTimer = 3f;
                    toBlack = true;
                }

                if(isRespawning)
                {

                    //  fade to black
                    if(toBlack)
                    {
                        respawnTimer -= dt;
                        Map.FadeLerp = (respawnTimer / 3f);

                        //  fade to black complete,  start fade to white
                        if(respawnTimer <= 0)
                        {
                            toBlack = false;
                            Position = Map.PlayerSpawnPos;
                            PlayAnimation(AnimationType.Idle);
                        }
                    }

                    //  fade to white
                    else
                    {
                        respawnTimer += dt;
                        Map.FadeLerp = (respawnTimer / 3f);

                        //  fade to white complete,  now alive
                        if(respawnTimer >= 3f)
                        {
                            IsAlive = true;
                            isRespawning = false;
                        }
                    }

                }

            }


        }

        public void RespawnEnd()
        {

        }

        public void Die(int dir)
        {
            PlayAnimation(AnimationType.PlayerDeath);
            IsAlive = false;

            Direction.X = dir;
            Direction.Y = -1;
            knockSpeed = 500f;

            deadTimer = 6f;
        }

        public void UpdateShooting(Map map, GameTime gt)
        {
            var dt = gt.Delta();
            if(ShootCooldownTimer < 0)
            {
                if(!IsShopping && !Builder.IsPlacing && !IsBuilding && IsAlive && !map.Game.mb.AnyUiHovered && !IsUpgradingOrReparing)
                    if(Input.LeftHold)
                    {
                        Shoot(map);
                        ShootCooldownTimer = ShootCooldown;
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

            var b = new Bullet(BulletType.Acorn, Center, des, 1, 0f, true);

            map.AddBullet(b);
            SoundManager.PlayerPlayerShoot();
        }

        public void Collision(List<Recc> recs)
        {

            //  map collision
            if(Rectangle.Left < Map.WallLeft - 35)
                Position = new Vector2(Map.WallLeft - 35, Position.Y);
            else if(Rectangle.Right + 50 > Map.WallRight)
                Position = new Vector2(Map.WallRight - Rectangle.Width - 50, Position.Y);

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

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if(IsAlive)
                DrawShadow(sb, 0, -28, 45, -15);
            else
                DrawShadow(sb, 0, -28, 65, -15);

            if(Globals.IsDebugging)
                sb.Draw(UtilityContent.box, CollisionBox, Color.Blue);
        }

    }
}
