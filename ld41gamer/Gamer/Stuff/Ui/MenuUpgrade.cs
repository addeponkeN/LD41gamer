using ld41gamer.Gamer.StateMachine.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{

    public enum UBBaseType
    {
        Branch,
        BuildSpeed,

    }

    public enum UType
    {
        Range,
        Shots,
        Damage,
        AttackSpeed,
    }

    public class UpgradeButton : Button
    {
        public UType Type;
        public Action Action;
        public UpgradeButton(Action a)
        {
            Action = a;
        }

        public void Update(GameTime gt, Map map)
        {
            if(IsReleased)
            {
                Action.Invoke();
            }
        }
    }

    public class UpgradeButtonBase : Button
    {
        public UBBaseType Type;
        public Action Action;

        public string Description;

        int[] costs;
        public int Cost => costs[Level];
        public int Level;

        public float Value;

        //public List<UpgradeButton> buttons;

        public UpgradeButtonBase(UBBaseType type, Action a)
        {
            CustomDraw = false;
            Action = a;

            Texture = GameContent.bigplank;
            font = GameContent.font24;

            this.Type = type;
            //buttons = new List<UpgradeButton>();

            switch(type)
            {

                case UBBaseType.Branch:

                    Text = "Buy Branch";
                    Set(50, 100, 175, 300);

                    break;

                case UBBaseType.BuildSpeed:

                    Text = "Upgrade Buildspeed";
                    Set(75, 125, 200, 400);

                    Action += () =>
                    {
                        Level++;
                        if(Level >= 3)
                        {
                            Upgrades.Player_BuildTimeMaxed = true;
                        }
                    };

                    break;

            }

        }

        void Set(params int[] cost)
        {
            costs = cost;
        }
        
    }

    public class MenuUpgrade
    {

        public List<UpgradeButtonBase> buttons;

        public MenuUpgrade()
        {
            buttons = new List<UpgradeButtonBase>();
        }

        public void AddButton(UBBaseType type, Action action)
        {
            int i = buttons.Count;
            var b = new UpgradeButtonBase(type, action);

            b.SetSize(256, 128);
            b.Position = new Vector2(100, 200 + (i * b.Size.Y) + (i * 16));

            buttons.Add(b);
        }

        public void Update(GameTime gt, GameStatePlaying gs)
        {

            foreach(var b in buttons)
            {
                if(b.IsReleased)
                {
                    b.Action.Invoke();
                }
            }

        }

        public void Draw(SpriteBatch sb, float lerp)
        {
            int arp = (int)MathHelper.Lerp(0, 255, lerp);

            foreach(var b in buttons)
            {
                float x = MathHelper.Lerp(-600, 100, lerp);
                b.Position = new Vector2(x, b.Position.Y);
                b.Draw(sb);
            }

        }
    }
}
