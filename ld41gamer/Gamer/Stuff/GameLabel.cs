using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class GameLabel : Label
    {

        public Sprite Item;
        public float timer;

        public GameLabel(Texture2D tex, string msg, Vector2 position, SpriteFont font) : base(font, msg)
        {
            Item = new Sprite(tex);
            SetPosition(position);
        }

        public void SetMoney(int temp)
        {
            timer = 1f;
        }

        public void SetPosition(Vector2 pos)
        {
            Item.Position = pos;
            Position = new Vector2(pos.X + Item.Size.X, GHelper.Center(Item.Rectangle, TextSize).Y);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            Item.Draw(sb);
        }
    }
}
