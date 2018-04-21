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


        Player player;


        public GameStatePlaying(GameScreen gs) : base(gs)
        {
            FadeInTime = 2f;
            FadeOutTime = 2f;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

            player = new Player();

        }

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);
            player.Update(gt, game);

        }

        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);
            sb.Begin();

            player.Draw(sb);

            sb.End();
        }

        public override void ExitState()
        {
            base.ExitState();

        }

    }
}
