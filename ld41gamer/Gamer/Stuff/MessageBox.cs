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
    public class MessageBox : GameLabel
    {

        public float LifeTimer;

        public MessageBox(string msg) : base(GameContent.acorn, msg, Vector2.Zero, GameContent.font24)
        {
            LifeTimer = 3;

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }
    }

    public class MBMan
    {
        public static List<MessageBox> list = new List<MessageBox>();


        public static void Add(string msg)
        {
            var m = new MessageBox(msg);

            list.Add(m);
        }

        public static void Update(GameTime gt)
        {
            for(int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                l.LifeTimer -= gt.Delta();
                if(l.LifeTimer <= 0)
                    list.RemoveAt(i);
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                var p = new Vector2(GHelper.Center(Globals.ScreenBox, l.Size + l.Item.Size).X, (Globals.ScreenCenter.Y - 200) * (l.LifeTimer/ 2.5f));
                l.SetPosition(p);
                l.Draw(sb);
            }

        }

    }
}
