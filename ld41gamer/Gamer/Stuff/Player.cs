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



        public Player()
        {
            Speed = 200;
        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            Direction = Vector2.Zero;

            if(Input.KeyHold(Keys.W))
            {
                Direction.Y = -1;
            }

            if(Input.KeyHold(Keys.S))
            {
                Direction.Y = 1;
            }

            if(Input.KeyHold(Keys.A))
            {
                Direction.X = -1;
            }

            if(Input.KeyHold(Keys.D))
            {
                Direction.X = 1;
            }


        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(UtilityContent.box, Rectangle, Color.MonoGameOrange);

        }

    }
}
