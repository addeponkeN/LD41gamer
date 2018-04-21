using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Obo.GameUtility;
using Obo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class CompassItem : Sprite
    {

        public Sprite enemyBox;

        public Enemy enemy;

        public bool IsDrawing;

        public CompassItem(Enemy en) : base(GameContent.compassbox)
        {
            enemy = en;
            SetSize(32);
            enemyBox = new Sprite(GameContent.enemysheet);
            enemyBox.SetSize(26);
            enemyBox.SetSourceSize(100);
            enemyBox.SetFrame(enemy.Row, enemy.Column);
            Origin = Size * .5f;
        }

        public void Update(GameTime gt, Camera2D cam, Player player)
        {
            if(cam.BoundingRectangle.Contains(enemy.Center))
                IsDrawing = false;
            else
                IsDrawing = true;

            Vector2 center = new Vector2(cam.BoundingRectangle.Center.X, cam.BoundingRectangle.Center.Y);

            //var ori = player.Rectangle.BotMiddle() - cam.Position;
            //var pos = cam.Position + ori;

            var dir = Vector2.Normalize(enemy.Center - player.Center);

            var rec = new Rectangle((int)cam.Position.X, (int)cam.Position.Y, 1280, 720);

            Position = enemy.Center;

            int x = (int)Helper.Clamp(Position.X, cam.Position.X + 32, rec.Width - (Size.X * 2));
            int y = (int)Helper.Clamp(Position.Y, cam.Position.Y + 32, rec.Height - (Size.Y * 2));

            Position = new Vector2(x, y);

            //int x = (int)MathHelper.Clamp((dir.X * (Globals.ScreenWidth * 2)), 0, Globals.ScreenWidth - Size.X);
            //int y = (int)MathHelper.Clamp((dir.Y * (Globals.ScreenHeight * 2)), 0, Globals.ScreenHeight - Size.Y);

            //Position = new Vector2(x, y);

            //var dir = Vector2.Normalize(enemy.Center - Position);

            Rotation = (float)Math.Atan2(dir.Y, dir.X) + MathHelper.PiOver2;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            enemyBox.Position = GHelper.Center(Rectangle, enemyBox.Size);
            enemyBox.Draw(sb);
        }

    }

    public class Compass
    {
        public List<CompassItem> Items;

        public Compass()
        {
            Items = new List<CompassItem>();

        }

        public void Add(Enemy en)
        {
            var c = new CompassItem(en);
            Items.Add(c);
        }

        public void Update(GameTime gt, Camera2D cam, Player p)
        {
            foreach(var c in Items)
            {
                c.Update(gt, cam, p);
            }
            Items.RemoveAll(x => !x.enemy.IsAlive);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(var c in Items)
            {
                if(c.IsDrawing)
                    c.Draw(sb);
            }
        }

    }
}
