using ld41gamer.Gamer.Screener;
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
    public class LivingObject : GameObject
    {

        public string Name { get; set; }
        public int HealthPoints { get; set; }
        public int MaxHealthPoints { get; set; }
        public int Damage { get; set; }

        public void SetHp(int hp)
        {
            HealthPoints = hp;
            MaxHealthPoints = hp;
        }
        public bool IsAlive { get; set; }

        HpBar HpBar;

        public LivingObject()
        {
            IsAlive = true;

        }

        public void CreateBar()
        {
            HpBar = new HpBar((int)Size.X/2, 10);

        }

        public override void Update(GameTime gt, Map map, GameScreen gs)
        {
            base.Update(gt, map, gs);

            HpBar?.Update(HealthPoints, MaxHealthPoints);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //HpBar.Position = new Vector2(GHelper.Center(Rectangle, HpBar.Size).X, Rectangle.Top);
            HpBar?.Draw(sb, new Vector2(GHelper.Center(Rectangle, HpBar.Size).X, Rectangle.Top));
        }



    }
}
