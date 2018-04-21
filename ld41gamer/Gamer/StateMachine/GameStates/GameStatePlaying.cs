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
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace ld41gamer.Gamer.StateMachine.GameStates
{
    public class GameStatePlaying : GameState
    {
        
        Player player;

        Map map;

        Camera2D cam2d;

        public GameStatePlaying(GameScreen gs) : base(gs)
        {
            FadeInTime = 2f;
            FadeOutTime = 2f;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

            player = new Player();
            map = new Map();

            var port = new ScalingViewportAdapter(game.ScreenManager.GraphicsDevice, 1280, 720);

            cam2d = new Camera2D(port);

        }

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            player.Update(gt, game);

            map.Update(gt, game);
            map.CheckCollision(player);

            cam2d.LookAt(player.Center);

        }

        void LockCam()
        {
            cam2d.LookAt(player.Center);

            //if()
            //if(cam2d.Position.Y + )
        }

        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);

            sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam2d.GetViewMatrix());

            map.Draw(sb);
            player.Draw(sb);

            sb.Draw(UtilityContent.box, new Rectangle(cam2d.Position.ToPoint(), new Point(2, 2)), Color.Red);

            sb.End();

        }

        public override void ExitState()
        {
            base.ExitState();

        }

    }
}
