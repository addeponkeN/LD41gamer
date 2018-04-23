using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        static float count = 1;

        public CompassItem(Enemy en) : base(GameContent.compassbox)
        {
            enemy = en;
            SetSize(42);

            var layer = Layer.Compass + (count / 5000);

            DrawLayer = layer;

            enemyBox = new Sprite(en.Texture);
            //enemyBox.SetSize(26);
            enemyBox.SetSourceSize(en.CurrentAnimation[0].Size.X, en.CurrentAnimation[0].Size.Y);
            enemyBox.Size = enemy.Size / 3;

            int row = 0;
            int col = enemy.Column;
            if(enemy.Type == EnemyType.WormRed || enemy.Type == EnemyType.WormYellow || enemy.Type == EnemyType.WormBlue)
                col = 1;

            if(enemy.Type == EnemyType.WormRed)
                row = 1;
            else if(enemy.Type == EnemyType.WormBlue)
                row = 2;

            enemyBox.SetFrame(col, row);
            enemyBox.DrawLayer = layer + 0.0001f;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            count += 1f;

            if(enemy.isAttacking)
                Color = Color.Red;
            else
                Color = Color.White;
        }

        public void Update(GameTime gt, Map map, Player player)
        {
            var cam = map.Game.cam2d;

            if(cam.BoundingRectangle.Contains(enemy.Center))
                IsDrawing = false;
            else
                IsDrawing = true;

            var target = enemy.CollisionBox.Center();

            Vector2 center = new Vector2(cam.BoundingRectangle.Center.X, cam.BoundingRectangle.Center.Y);

            var dir = Vector2.Normalize(target - player.Center);

            var rec = cam.BoundingRectangle;

            Position = target;

            int x = (int)Helper.Clamp(Position.X, cam.Position.X + 32, rec.Right - (Size.X / 2));
            int y = (int)Helper.Clamp(Position.Y, cam.Position.Y + 32, rec.Bottom - (Size.Y / 2));

            Position = new Vector2(x, y);

            Rotation = (float)Math.Atan2(dir.Y, dir.X) + MathHelper.PiOver2;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            var rec = new Rectangle(Rectangle.X - ((int)Size.X / 2), Rectangle.Y - ((int)Size.Y / 2), (int)Size.X, (int)Size.Y);
            var c = enemy.CollisionSize / 3;
            var p = new Vector2(GHelper.Center(rec, c).X, rec.Y + (rec.Height / 2) - enemyBox.Size.Y + c.Y);

            enemyBox.Position = p;
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

        public void Update(GameTime gt, Map map, Player p)
        {
            foreach(var c in Items)
            {
                c.Update(gt, map, p);
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
