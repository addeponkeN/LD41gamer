using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using Obo.Gui;
using ld41gamer.Gamer.Screener;

namespace ld41gamer.Gamer.StateMachine.GameStates
{
    public class GameStatePlaying : GameState
    {



        public GameStatePlaying(GameScreen gs) : base(gs)
        {
            FadeInTime = 2f;
            FadeOutTime = 2f;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

        }

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

        }

        //  update only when not paused
        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

        }

        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);
            sb.Begin();



            sb.End();
        }

        public override void ExitState()
        {
            base.ExitState();

        }

    }
}
