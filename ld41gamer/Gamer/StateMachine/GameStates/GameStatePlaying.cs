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

    public enum GameStater
    {
        Level1,

        Level2,

        Level3,

        Level4,

    }

    public class GameStatePlaying : GameState
    {

        public Map map;

        public Camera2D cam2d;

        float lerpTimer = 1f;

        GameLabel lbMoney;

        public MenuUpgrade mu;
        public MenuBuy mb;
        float muLerp;

        public bool AnyUiHovered => mu.buttons.Any(x => x.IsHovered);

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

            mu = new MenuUpgrade();
            mb = new MenuBuy(game.ScreenManager.GraphicsDevice);

            mu.AddButton(UBBaseType.Branch, () =>
            {
                if(!Upgrades.TreeBranchesMaxed)
                    map.player.IsShoppingBranch = !map.player.IsShoppingBranch;
            });

            mu.AddButton(UBBaseType.BuildSpeed, () =>
            {
                if(!Upgrades.Player_BuildTimeMaxed)
                    map.player.BuildSpeed += 0.5f;
            });


        }

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            mu.Update(gt, this);

            map.Update(gt, game);

            lbMoney.Text = map.player.Money.ToString();

            var dt = gt.Delta();

            if(map.player.IsShopping)
            {
                if(muLerp < 1f)
                    muLerp += dt;
            }
            else if(muLerp >= 0f)
                muLerp -= dt;

            float zout = 0.33f;
            float zin = 1f;

            Vector2 pos;

            float x;
            float y;

            if(map.player.IsShopping)
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

            if(map.player.IsShopping || lerpTimer < 1f)
            {
                cam2d.Zoom = MathHelper.Lerp(zout, zin, lerpTimer);
            }

            x = MathHelper.Lerp(map.tree.Center.X, map.player.Center.X, lerpTimer);
            y = MathHelper.Lerp(map.tree.Center.Y, map.player.Center.Y, lerpTimer);

            pos = new Vector2(x, y + 1);
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
            int left = Map.WallLeft;
            int right = Map.WallRight;

            if(cam2d.BoundingRectangle.Bottom > map.GroundRectangle.Bottom)
            {
                int y = (int)(cam2d.BoundingRectangle.Bottom - map.GroundRectangle.Bottom);
                cam2d.Position = new Vector2(cam2d.Position.X, cam2d.Position.Y - y);
            }

            if(cam2d.BoundingRectangle.Left < left)
            {
                //int x = (int)(cam2d.BoundingRectangle.Bottom - map.GroundRectangle.Bottom);
                cam2d.Position = new Vector2(left, cam2d.Position.Y);
            }

            if(cam2d.BoundingRectangle.Right > right)
            {
                //int x = (int)(cam2d.BoundingRectangle.Bottom - map.GroundRectangle.Bottom);
                cam2d.Position = new Vector2(right - cam2d.BoundingRectangle.Width, cam2d.Position.Y);
            }
        }


        int i = 0;
        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);
            
            //  WORLD LAYER
            sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam2d.GetViewMatrix());

            map.DrawWorld(sb);

            sb.End();

            //  WORLD
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam2d.GetViewMatrix());

            map.DrawTowerRecs(sb);
            map.DrawDef(sb);

            sb.End();


            //  SCREEN
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

            lbMoney.Draw(sb);

            map.DrawScreen(sb);

            i = 0;
            var mpos = map.MouseWorldPos();
            DrawString(sb, $"Mouse: X:{(int)mpos.X}  Y:{(int)mpos.Y}");
            DrawString(sb, $"PlayerCenter: X:{(int)map.player.Center.X}  Y:{(int)map.player.Center.Y}");
            DrawString(sb, $"Camera: X:{(int)cam2d.Position.X}  Y:{(int)cam2d.Position.Y}");
            DrawString(sb, $"Camera: W:{(int)cam2d.BoundingRectangle.Right}  H:{(int)cam2d.BoundingRectangle.Left}");

            if(muLerp > 0.01f)
                mu.Draw(sb, muLerp);

            mb.Draw(sb, 1f);

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
