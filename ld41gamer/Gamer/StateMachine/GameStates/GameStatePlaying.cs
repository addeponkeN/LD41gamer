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

        float lerpTimer = 1f;

        GameLabel lbMoney;


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
            cam2d.MinimumZoom = 0.1f;
            cam2d.MaximumZoom = 3.1f;

            lbMoney = new GameLabel(GameContent.acorn, "0", new Vector2(Globals.ScreenWidth * 0.85f, 8));
            lbMoney.Item.Size = new Vector2(64);

        }

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);


            map.Update(gt, game);

            lbMoney.Text = map.player.Money.ToString();

            var dt = gt.Delta();


            float zout = 0.33f;
            float zin = 1f;

            Vector2 pos;

            float x;
            float y;

            if(map.player.IsBuying)
            {
                lerpTimer -= dt * 1.25f;
                if(lerpTimer <= 0f)
                    lerpTimer = 0f;
            }
            else
            {
                lerpTimer += dt * 1.25f;
                if(lerpTimer >= 1f)
                    lerpTimer = 1f;
            }

            if(map.player.IsBuying || lerpTimer < 1f)
            {
                cam2d.Zoom = MathHelper.Lerp(zout, zin, lerpTimer);
            }

            x = MathHelper.Lerp(map.tree.Center.X, map.player.Center.X, lerpTimer);
            y = MathHelper.Lerp(map.tree.Center.Y, map.player.Center.Y, lerpTimer);

            pos = new Vector2(x, y);
            cam2d.LookAt(pos);

            if(Input.WheelDown)
            {
                cam2d.ZoomOut(0.01f);
            }
            else if(Input.WheelUp)
            {
                cam2d.ZoomIn(0.01f);
            }

            Input.ScrollValueOld = Input.ScrollValue;

            //if(lerpTimer > zin-0.1f)
            LockCamToMap(map);

        }

        void LockCamToMap(Map map)
        {

            if(cam2d.BoundingRectangle.Bottom > map.GroundRectangle.Bottom)
            {
                int y = (int)(cam2d.BoundingRectangle.Bottom - map.GroundRectangle.Bottom);
                cam2d.Position = new Vector2(cam2d.Position.X, cam2d.Position.Y - y);
            }

        }

        int i = 0;
        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam2d.GetViewMatrix());

            map.DrawWorld(sb);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

            lbMoney.Draw(sb);

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
