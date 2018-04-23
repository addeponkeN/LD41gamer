using ld41gamer.Gamer.Sprites;
using ld41gamer.Gamer.StateMachine.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer.Stuff.Ui
{
    public class BuyUi : Button
    {

        public Sprite Item;

        public BuyUi(GraphicsDevice gd) : base(gd, new Vector2(32, 32), "")
        {


        }


    }

    public class MenuBuy
    {

        public List<BuyUi> uis;

        
        BuyUi btCancel;
        BuyUi btTurret;
        BuyUi btSniper;
        BuyUi btCata;

        public MenuBuy(GraphicsDevice gd)
        {
            uis = new List<BuyUi>();
            //AcornTurret,
            //AcornSniper,
            //ConeCatapult

            btTurret = new BuyUi(gd);
            btTurret.Item = new Sprite(GameContent.turretsheet);
            btTurret.Item.SetSize(28);
            btTurret.Item.SetSourceSize(192);
            btTurret.Item.SetFrame(0, 0);


            btTurret = new BuyUi(gd);
            btTurret.Item = new Sprite(GameContent.turretsheet);
            btTurret.Item.SetSize(28);
            btTurret.Item.SetSourceSize(192);
            btTurret.Item.SetFrame(0, 0);

            btSniper = new BuyUi(gd);
            btSniper.Item = new Sprite(GameContent.turretsheet);
            btSniper.Item.SetSize(28);
            btSniper.Item.SetSourceSize(192);
            btSniper.Item.SetFrame(1, 0);


            btCata = new BuyUi(gd);
            btCata.Item = new Sprite(GameContent.turretsheet);
            btCata.Item.SetSize(28);
            btCata.Item.SetSourceSize(192);
            btCata.Item.SetFrame(2, 0);

            uis.Add(btTurret);
            uis.Add(btSniper);
            uis.Add(btCata);
        }

        public void Update(GameTime gt, GameStatePlaying gs)
        {


        }

        public void Draw(SpriteBatch sb, float lerp)
        {
            foreach(var b in uis)
            {


            }

        }

    }
}
