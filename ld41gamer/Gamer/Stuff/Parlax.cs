﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{

    public class Par
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 BasePos;
        public Rectangle Rectangle => new Rectangle((int)Position.X + (int)OffSetX, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public float OffSetX;
    }

    public class Parlax
    {
        public List<Par> list = new List<Par>();

        public Parlax(Map map)
        {
            Add(new Vector2(0, 0), map.BoxRectangle.Width, map.BoxRectangle.Height);
            Add(new Vector2(0, Map.GroundCollisionBox.Top - 512), 1280, 512);
            Add(new Vector2(0, Map.GroundCollisionBox.Top - 700), 900, 700);
            Add(new Vector2(0, Map.GroundCollisionBox.Top - 202 * 4), 858 * 4, 202 * 4);
        }

        void Add(Vector2 pos, float w, float h)
        {
            list.Add(new Par() { Size = new Vector2(w, h), Position = pos, BasePos = pos });
        }

        public void Update(GameTime gt, Map map)
        {
            var dt = gt.Delta();
            //if(map.player.IsMoving)
            //    for(int i = 0; i < 4; i++)
            //    {
            //        var p = list[i];
            //        float speed = (map.player.Speed / (4 - i)) * 0.35f;
            //        p.OffSetX += speed * (-map.player.Direction.X / 2) * dt;
            //    }
        }


        public void Draw(SpriteBatch sb, Map map)
        {

            var disX = Vector2.Distance(new Vector2(Map.WallLeft, map.Game.cam2d.Position.Y), map.Game.cam2d.Position);
            var disY = Vector2.Distance(new Vector2(map.Game.cam2d.Position.X, map.tree.Rectangle.Bottom), new Vector2(map.Game.cam2d.Position.X, map.Game.cam2d.BoundingRectangle.Bottom - 100));
            for(int i = 0; i < 4; i++)
            {
                var p = list[i];

                //var lp = (Map.PlayerSpawnPos.X / map.player.Center.X);


                switch(i)
                {
                    case 0:

                        for(float x = 0; x < map.BoxRectangle.Right; x += p.Rectangle.Width)
                        {
                            p.Position.X = x + (disX * 0.8f);
                            p.Position.Y = p.BasePos.Y + (disY / 20);
                            sb.Draw(GameContent.layer0, p.Rectangle, Color.White, Layer.BACK + 0.10f);
                        }

                        break;
                    case 1:

                        for(float x = 0; x < map.BoxRectangle.Right - (p.OffSetX * 2); x += p.Rectangle.Width)
                        {
                            p.Position.X = x + (disX * 0.70f);
                            p.Position.Y = p.BasePos.Y - (disY / 4);
                            sb.Draw(GameContent.layer1, p.Rectangle, Color.White, Layer.BACK + 0.11f);
                        }

                        break;
                    case 2:

                        for(float x = 0; x < map.BoxRectangle.Right - (p.OffSetX * 2); x += p.Rectangle.Width)
                        {
                            p.Position.X = x + (disX * 0.50f);
                            p.Position.Y = p.BasePos.Y - (disY / 6);
                            sb.Draw(GameContent.layer2, p.Rectangle, Color.White, Layer.BACK + 0.12f);
                        }

                        break;
                    case 3:
                        for(float x = 0; x < map.BoxRectangle.Right; x += p.Rectangle.Width)
                        {
                            p.Position.X = x - (disX * 0.03f);
                            p.Position.Y = p.BasePos.Y + (disY / 5);
                            sb.Draw(GameContent.layer3, p.Rectangle, Color.White, Layer.BACK + 0.13f);
                        }

                        break;
                }
            }
        }
    }
}
