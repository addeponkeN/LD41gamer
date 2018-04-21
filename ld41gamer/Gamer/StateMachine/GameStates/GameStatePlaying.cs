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

        public Map map;

        public Camera2D cam2d;

        public GameStatePlaying(GameScreen gs) : base(gs)
        {
            FadeInTime = 2f;
            FadeOutTime = 2f;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

            map = new Map(this);

            var port = new ScalingViewportAdapter(game.ScreenManager.GraphicsDevice, 1280, 720);
            cam2d = new Camera2D(port);

        }

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            LockCam(map);

            map.Update(gt, game);

        }

        void LockCam(Map map)
        {
            cam2d.LookAt(map.player.Center);

            if(cam2d.BoundingRectangle.Bottom > map.GroundRectangle.Bottom)
            {
                cam2d.Position = new Vector2(cam2d.Position.X, map.GroundRectangle.Bottom - cam2d.BoundingRectangle.Height);
            }

        }
        int i = 0;
        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam2d.GetViewMatrix());

            map.DrawWorld(sb);

            //sb.Draw(UtilityContent.box, new Rectangle(cam2d.Position.ToPoint(), new Point(2, 2)), Color.Red);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

            map.DrawScreen(sb);

            i = 0;
            var mpos = map.MouseWorldPos();
            DrawString(sb, $"Mouse: X:{(int)mpos.X}  Y:{(int)mpos.Y}");
            DrawString(sb, $"PlayerCenter: X:{(int)map.player.Center.X}  Y:{(int)map.player.Center.Y}");
            DrawString(sb, $"Camera: X:{(int)cam2d.Position.X}  Y:{(int)cam2d.Position.Y}");
            DrawString(sb, $"Camera: W:{(int)cam2d.BoundingRectangle.Right}  H:{(int)cam2d.BoundingRectangle.Left}");

            sb.End();
        }

        void DrawString(SpriteBatch sb, string msg)
        {
            Extras.DrawString(sb, UtilityContent.debugFont, msg, new Vector2(4, 100 + (20 * i)));
            i++;
        }

        public override void ExitState()
        {
            base.ExitState();

        }

    }
}
