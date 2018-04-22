using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class HpBar : Sprite
    {
        public float Value;
        public float MaxValue;

        public float Percent => (Value / MaxValue);

        public Sprite Foreground;
        public Sprite Background;

        new public Rectangle Rectangle => new Rectangle((int)Position.X - gap, (int)Position.Y-gap, (int)(Size.X * Globals.Scale.X) + (gap*2), (int)(Size.Y * Globals.Scale.Y) + (gap * 2));

        int gap = 2;

        public HpBar(int w, int h)
        {
            SetColor(Color.Black);
            SetSize(w, h);

            Foreground = new Sprite();
            Foreground.SetSize(w, h);
            Foreground.Color = Color.ForestGreen;

            Background = new Sprite();
            Background.SetSize(w, h);
            Background.Color = Color.IndianRed;
        }

        public void Update(float value, float maxValue)
        {
            Value = value;
            MaxValue = maxValue;

            Foreground.Size = new Vector2(Percent * Size.X, Foreground.Size.Y);
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            Position = position;
            Background.Position = position;
            Foreground.Position = position;

            sb.Draw(Texture, Rectangle, FrameRectangle, new Color(Color, Alpha), Rotation, Origin, SpriteEffects, Layer);

            Background.Draw(sb);
            Foreground.Draw(sb);

        }
    }
}
