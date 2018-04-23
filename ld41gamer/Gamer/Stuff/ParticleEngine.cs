using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public enum ParticleType
    {
        Smoke,
        Blood,
        Scrap,
    }

    public class Particle
    {
        public ParticleType Type;

        public Vector2 Position;
        public Vector2 Size;
        public Vector2 BaseSize;

        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Globals.Scale.X), (int)(Size.Y * Globals.Scale.Y));

        public int Column = 0;
        public int Row = 0;

        public Rectangle FrameRectangle => new Rectangle(Column * 32, Row * 32, 32, 32);

        public int Alpha = 255;
        public int BaseAlpha;
        public Color Color = Color.White;
        public Color BaseColor;

        public float Rotation;
        public float RotationSpeed;

        public Vector2 Origin;
        public SpriteEffects SpriteEffects;

        public float LifeTime;
        public float LifeTimeBase;

        public float FadeInTimer = 0f;
        public float FadeInTimerBase;

        float FadeInLerp = 1f;

        public Vector2 Speed;

        public Vector2 Direction;

        public Vector2 endPos;

        float growY;

        public float DrawLayer;

        void SetColor(int val)
        {
            Color = new Color(val, val, val);
            Alpha = val;
        }

        public Particle(ParticleType type, Vector2 startPos, Vector2 dir)
        {
            Type = type;
            DrawLayer = Layer.Particle;
            Size = new Vector2(32);
            Origin = Size / 2;

            Direction = dir;

            float pi = (float)Math.PI;

            switch(Type)
            {
                case ParticleType.Smoke:
                    Row = 0;
                    Column = Rng.Noxt(0, 2);
                    Speed = new Vector2(Rng.Noxt(10, 35));
                    LifeTime = Rng.NoxtFloat(2.5f, 4.5f);

                    RotationSpeed = Rng.NoxtFloat(-pi, pi);
                    FadeInTimer = 0.33f;
                    Size = new Vector2(64);

                    SetColor(130);

                    break;

                case ParticleType.Blood:
                    Row = 1;
                    Column = Rng.Noxt(0, 1);

                    LifeTime = 15f;
                    Speed = new Vector2(50);
                    FadeInTimer = 0.25f;

                    RotationSpeed = Rng.NoxtFloat(-pi * 2, pi * 2);

                    Size = new Vector2(Rng.Noxt(8, 24));
                    SetColor(255);
                    break;


                case ParticleType.Scrap:
                    Row = 2;
                    Column = Rng.Noxt(0, 3);
                    SetColor(255);
                    LifeTime = 8f;
                    Speed = new Vector2(Rng.Noxt(80, 150), Rng.Noxt(50,300));
                    FadeInTimer = 0f;

                    RotationSpeed = Rng.NoxtFloat(-pi * 3, pi * 3);

                    Size = new Vector2(Rng.Noxt(8, 24));

                    break;

                default:
                    Console.WriteLine("EERRORR !! NO SUCH PARTICLE TYPE");
                    break;

            }

            Position = startPos - (Size / 2);

            SpriteEffects = RandomEffect();

            LifeTimeBase = LifeTime;
            BaseColor = Color;
            BaseAlpha = Alpha;
            BaseSize = Size;
            FadeInTimerBase = FadeInTimer;
            FadeInTimer = 0f;

            // Origin = new Vector2((Size.X / 2) + (Column * 32), (Size.Y / 2) + (Row * 32));
            //Origin = Size / 2;
        }

        public void FadeLerp(float val)
        {
            Color = Color.Lerp(Color.Black, BaseColor, val);
            Alpha = (int)MathHelper.Lerp(0, BaseAlpha, val);
        }

        public void UpdatePos(float dt)
        {
            Position += Direction * Speed * dt;
        }

        public SpriteEffects RandomEffect()
        {
            int rng = Rng.Noxt(3);
            if(rng == 0)
                return SpriteEffects.None;
            else if(rng == 1)
                return SpriteEffects.FlipHorizontally;
            else if(rng == 2)
                return SpriteEffects.FlipVertically;
            else
                return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
        }

        public void Rotate(float dt)
        {
            Rotation += dt * RotationSpeed;
        }

        public void Update(GameTime gt, Map map)
        {
            var dt = gt.Delta();
            LifeTime -= dt;

            if(LifeTime > 1f)
                if(FadeInTimer <= FadeInTimerBase)
                {
                    FadeInTimer += dt;
                    FadeInLerp = MathHelper.Lerp(0f, 1f, FadeInTimer / FadeInTimerBase);
                    FadeLerp(FadeInLerp);
                }

            switch(Type)
            {

                case ParticleType.Smoke:

                    Rotate(dt);

                    UpdatePos(dt);

                    break;


                case ParticleType.Blood:

                    if(Rectangle.Bottom < endPos.Y)
                    {
                        UpdatePos(dt);
                        Direction.Y += dt * 4f;
                        Rotate(dt);
                    }
                    else
                    {
                        DrawLayer = Layer.Tree + 0.005f;
                        growY += dt * 4f;
                    }

                    break;

                case ParticleType.Scrap:

                    if(Rectangle.Bottom < endPos.Y)
                    {
                        UpdatePos(dt);
                        Speed.Y -= dt * 200f;
                        if(Speed.X > 1f)
                        Speed.X -= dt * 5f;
                        Rotate(dt);
                    }
                    else
                    {
                        DrawLayer = Layer.Tree + 0.005f;
                    }

                    break;
            }


            //  fade out on death
            if(LifeTime < 1f)
                FadeLerp(LifeTime);

        }

        public static Vector2 RandomDir()
        {
            return Vector2.Normalize(new Vector2(Rng.NoxtFloat(-1, 1), Rng.NoxtFloat(-1, 1)));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(GameContent.particlesheet, Rectangle, FrameRectangle, new Color(Color, Alpha), Rotation, Origin, SpriteEffects, DrawLayer);

            switch(Type)
            {
                case ParticleType.Smoke:
                    break;
                case ParticleType.Blood:
                    sb.Draw(GameContent.particlesheet, 
                        new Rectangle((int)Position.X - 3, (int)Position.Y - (int)(growY * .29f), 6, (int)growY), 
                        FrameRectangle, new Color(Color, Alpha), 0f, Vector2.Zero, SpriteEffects, Layer.Tree + 0.004f);
                    break;
                default:
                    break;
            }

        }
    }

    public class ParticleEngine
    {
        public List<Particle> list;

        public ParticleEngine()
        {
            list = new List<Particle>();
        }

        public void Add(Particle p)
        {
            list.Add(p);
        }

        public void Add(ParticleType type, Vector2 spawnPos, Vector2 direction)
        {
            var p = new Particle(type, spawnPos, direction);
            list.Add(p);
        }

        public void Update(GameTime gt, Map map)
        {

            for(int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                p.Update(gt, map);

                if(p.LifeTime < 0f)
                    list.RemoveAt(i);
            }

        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                p.Draw(sb);
            }
        }

    }
}
