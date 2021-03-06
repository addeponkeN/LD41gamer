﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ld41gamer.Gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Utility;

namespace ld41gamer.Gamer
{

    public enum BulletType
    {

        Acorn,

        Cone,

    }

    public class Bullet : GameObject
    {
        public float AirVelo;
        public BulletType Type;

        public float LifeTime;
        public float BulletDrop;

        Vector2 oldPos;

        public int Damage = 1;

        float Angle;
        public int Pierce;

        Vector2 Destination;
        public List<int> hits = new List<int>();

        float rotationSpeed;

        public Bullet(BulletType t, Vector2 spawnPos, Vector2 destination, int damage, int pierce, float xbet = 0f, bool isPlayer = false)
        {
            Type = t;
            Position = spawnPos;
            Direction = Vector2.Normalize(destination - spawnPos);
            Destination = destination;
            Damage = damage;

            if(isPlayer)
                SetSize(16);
            else
                SetSize(28);
            SetSourceSize(100);
            SetFrame(0, 0);
            IsAnimating = false;
            Pierce = pierce;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            LifeTime = 20f;

            DrawLayer = Layer.Bullet;

            switch(Type)
            {

                case BulletType.Acorn:
                    if(Speed == 0)
                        Speed = 600f + xbet;

                    Direction = Vector2.Normalize((destination - new Vector2(0, xbet)) - spawnPos);

                    AirVelo = -45f;
                    Texture = GameContent.acorn;
                    BulletDrop = 200f;

                    break;

                case BulletType.Cone:

                    var dis = Vector2.Distance(spawnPos, destination);

                    //AirVelo = -(xbet * .1f);
                    //BulletDrop = (xbet * .1f);
                    //Speed = dis*0.075f;

                    AirVelo = -300f;
                    BulletDrop = 300f;
                    Speed = 200f + (dis / 5);

                    rotationSpeed = Rng.NoxtFloat(-(float)MathHelper.Pi, (float)MathHelper.Pi);

                    Texture = GameContent.cone;

                    break;

            }

            if(isPlayer)
            {
                BulletDrop = 650f;
                Speed += 120;
            }

            //RotateToDestination();

        }

        void Gravity(float dt)
        {
            AirVelo += BulletDrop * dt;
            Position += new Vector2(0, AirVelo * dt);
        }

        public override void UpdatePosition(GameTime gt)
        {
            base.UpdatePosition(gt);
        }

        public void RotateToDestination()
        {
            Rotation = Angle;
        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);
            var dt = gt.Delta();

            LifeTime -= dt;

            switch(Type)
            {

                case BulletType.Acorn:

                    Gravity(dt);
                    UpdatePosition(gt);
                    RotateToDestination();

                    break;

                case BulletType.Cone:

                    Gravity(dt);
                    UpdatePosition(gt);

                    Rotation += dt * rotationSpeed;
                    break;

            }

            var dir = Vector2.Normalize(oldPos - Position);
            Angle = (float)Math.Atan2(dir.Y, dir.X) + MathHelper.PiOver2;
            oldPos = Position;

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(Texture, Rectangle, )
        }

    }


}
