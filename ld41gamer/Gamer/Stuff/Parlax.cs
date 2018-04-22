using Microsoft.Xna.Framework;
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
        public Rectangle Rectangle => new Rectangle((int)Position.X + (int)OffSetX, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public float OffSetX;
    }

    public class Parlax
    {
        public List<Par> list = new List<Par>();

        public Parlax(Map map)
        {
            Add(new Vector2(0, 0), map.BoxRectangle.Width, map.BoxRectangle.Height);
            Add(new Vector2(0, map.GroundCollisionBox.Top - 512), 1280, 512);
            Add(new Vector2(0, map.GroundCollisionBox.Top - 700), 900, 700);
            Add(new Vector2(0, map.GroundCollisionBox.Top - 202 * 4), 858 * 4, 202 * 4);
        }

        void Add(Vector2 pos, float w, float h)
        {
            list.Add(new Par() { Size = new Vector2(w, h), Position = pos });
        }

        public void Update(GameTime gt, Map map)
        {
            var dt = gt.Delta();
            if(map.player.IsMoving)
                for(int i = 0; i < 4; i++)
                {
                    var p = list[i];
                    float speed = (map.player.Speed / (4 - i)) * 0.35f;
                    p.OffSetX += speed * (-map.player.Direction.X / 2) * dt;
                }
        }


        public void Draw(SpriteBatch sb, Map map)
        {

            for(int i = 0; i < 4; i++)
            {
                var p = list[i];
                switch(i)
                {
                    case 0:

                        for(float x = 0; x < map.BoxRectangle.Right - p.OffSetX; x += p.Rectangle.Width)
                        {
                            p.Position.X = x;
                            sb.Draw(GameContent.layer0, p.Rectangle, Color.White);
                        }

                        break;
                    case 1:

                        for(float x = 0; x < map.BoxRectangle.Right - (p.OffSetX * 2); x += p.Rectangle.Width)
                        {
                            p.Position.X = x;
                            sb.Draw(GameContent.layer1, p.Rectangle, Color.White);
                        }

                        break;
                    case 2:

                        for(float x = 0; x < map.BoxRectangle.Right - (p.OffSetX * 2); x += p.Rectangle.Width)
                        {
                            p.Position.X = x;
                            sb.Draw(GameContent.layer2, p.Rectangle, Color.White);
                        }

                        break;
                    case 3:
                        for(float x = 0; x < map.BoxRectangle.Right - (p.OffSetX * 2); x += p.Rectangle.Width)
                        {
                            p.Position.X = x;
                            sb.Draw(GameContent.layer3, p.Rectangle, Color.White);
                        }

                        break;
                }
            }
        }
    }
}
