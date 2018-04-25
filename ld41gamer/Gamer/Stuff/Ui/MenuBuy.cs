using ld41gamer.Gamer.Sprites;
using ld41gamer.Gamer.StateMachine.GameStates;
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
    public class BuyUi : Button
    {

        public Sprite Item;
        public Label lb;

        public BuyUi(GraphicsDevice gd, string key) : base(gd, new Vector2(32, 32), "")
        {
            SetSize(100);
            lb = new Label(GameContent.font14, Vector2.Zero, Color.White, key);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            Item.Draw(sb);
            lb.Draw(sb);
        }

    }

    public class MenuBuy
    {

        public List<BuyUi> uis;

        public BuyUi btShoot;
        public BuyUi btDelete;
        public BuyUi btCancel;
        public BuyUi btTurret;
        public BuyUi btSniper;
        public BuyUi btCata;

        float tot;

        public bool AnyUiHovered => uis.Any(x => x.IsHovered);

        public MenuBuy(GraphicsDevice gd)
        {
            uis = new List<BuyUi>();

            int size = 100;

            Texture2D tex = GameContent.btplank;

            btShoot = new BuyUi(gd, 
                "Mouse Left" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "Shoot");
            btShoot.Texture = tex;
            btShoot.Item = new Sprite(GameContent.acorn);
            btShoot.Item.SetSize(80);
            btShoot.Item.SetSourceSize(100);
            btShoot.Item.SetFrame(0, 0);

            btCancel = new BuyUi(gd, 
                "Mouse Right" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "Cancel");
            btCancel.Texture = tex;
            btCancel.Item = new Sprite(GameContent.icons);
            btCancel.Item.SetSize(80);
            btCancel.Item.SetSourceSize(64);
            btCancel.Item.SetFrame(3, 0);

            btDelete = new BuyUi(gd, 
                "Q" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "Demolish");
            btDelete.Texture = tex;
            btDelete.Item = new Sprite(GameContent.icons);
            btDelete.Item.SetSize(80);
            btDelete.Item.SetSourceSize(64);
            btDelete.Item.SetFrame(4, 0);

            btTurret = new BuyUi(gd, "1");
            btTurret.Texture = tex;
            btTurret.Item = new Sprite(GameContent.turretsheet);
            btTurret.Item.SetSize(size);
            btTurret.Item.SetSourceSize(192);
            btTurret.Item.SetFrame(0, 0);

            btSniper = new BuyUi(gd,"2");
            btSniper.Texture = tex;
            btSniper.Item = new Sprite(GameContent.turretsheet);
            btSniper.Item.SetSize(size);
            btSniper.Item.SetSourceSize(192);
            btSniper.Item.SetFrame(0, 1);

            btCata = new BuyUi(gd,"3");
            btCata.Texture = tex;
            btCata.Item = new Sprite(GameContent.turretsheet);
            btCata.Item.SetSize(size);
            btCata.Item.SetSourceSize(192);
            btCata.Item.SetFrame(0, 2);

            uis.Add(btShoot);
            uis.Add(btTurret);
            uis.Add(btSniper);
            uis.Add(btCata);

            uis.Add(btCancel);
            uis.Add(btDelete);

            tot = (uis.Count * btTurret.Size.X) + (uis.Count * 4);
        }

        public void Update(GameTime gt, GameStatePlaying gs)
        {


        }

        public void Draw(SpriteBatch sb, float lerp)
        {
            for(int i = 0; i < uis.Count; i++)
            {
                var b = uis[i];

                float y = MathHelper.Lerp(Globals.ScreenHeight + 50, Globals.ScreenHeight - b.Size.Y - 4, lerp);

                b.Position = new Vector2((Globals.ScreenWidth / 3) + (i * b.Size.X) + (i * 4), y);
                b.Item.Position = GHelper.Center(b.Rectangle, b.Item.Size);
                b.lb.Position = b.Position + new Vector2(4);
                b.Draw(sb);
            }

        }

    }
}
