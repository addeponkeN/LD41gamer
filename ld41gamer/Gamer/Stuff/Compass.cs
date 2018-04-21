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
