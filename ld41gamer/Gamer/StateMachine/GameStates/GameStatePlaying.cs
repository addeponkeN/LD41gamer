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
        GameLabel lbTurret;
        GameLabel lbSniper;
        GameLabel lbCata;

        public MenuUpgrade mu;
        public MenuBuy mb;

        public TreeHp treeBar;

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


            var port = new ScalingViewportAdapter(game.ScreenManager.GraphicsDevice, 1280, 720);
            cam2d = new Camera2D(port);
            cam2d.MinimumZoom = 0.1f;
            cam2d.MaximumZoom = 3.1f;

            map = new Map(this);


            lbMoney = new GameLabel(GameContent.acorn, "0", new Vector2(Globals.ScreenWidth * 0.85f, 8), GameContent.font48);
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


            var pos = mb.btTurret.Position - new Vector2(0, 14);
            lbTurret = new GameLabel(GameContent.acorn, "10", pos, GameContent.font14);
            lbTurret.Item.SetSize(32);

            pos = mb.btTurret.Position - new Vector2(0, 14);
            lbSniper = new GameLabel(GameContent.acorn, "30", pos, GameContent.font14);
            lbSniper.Item.SetSize(32);

            pos = mb.btTurret.Position - new Vector2(0, 14);
            lbCata = new GameLabel(GameContent.acorn, "100", pos, GameContent.font14);
            lbCata.Item.SetSize(32);


            treeBar = new TreeHp();
            treeBar.Posser(new Vector2(8, 8));

        }

        public Vector2 camDest;

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            mu.Update(gt, this);

            map.Update(gt, game);

            lbMoney.Text = map.player.Money.ToString();

            treeBar.Update(map.tree.HealthPoints, map.tree.MaxHealthPoints);


            if(map.player.Money < Turret.TurretCost)
            {
                lbTurret.Color = Color.IndianRed;
            }
            else
                lbTurret.Color = Color.ForestGreen;

            if(map.player.Money < Turret.SniperCost)
            {
                lbSniper.Color = Color.IndianRed;
            }
            else
                lbSniper.Color = Color.ForestGreen;

            if(map.player.Money < Turret.CataCost)
            {
                lbCata.Color = Color.IndianRed;
            }
            else
                lbCata.Color = Color.ForestGreen;


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



            //if(Builder.IsPlacing && map.builder.b != null)
            //{
            //    camDest = map.player.Center;
            //    lerpTimer -= dt * 1.25f;
            //    if(lerpTimer <= 0f)
            //        lerpTimer = 0f;
            //}
            if(map.player.IsShopping/* || Builder.IsPlacing*/)
            {
                camDest = map.tree.Center;
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
            
            x = MathHelper.Lerp(camDest.X, map.player.Center.X, lerpTimer);
            y = MathHelper.Lerp(camDest.Y, map.player.Center.Y, lerpTimer);

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

        public void LockCamToMap(Map map)
        {
            int left = Map.WallLeft;
            int right = Map.WallRight;

            if(cam2d.BoundingRectangle.Bottom > map.GroundRectangle.Bottom - 2)
            {
                int y = (int)(cam2d.BoundingRectangle.Bottom - map.GroundRectangle.Bottom - 2);
                cam2d.Position = new Vector2(cam2d.Position.X, cam2d.Position.Y - y);
            }

            if(cam2d.BoundingRectangle.Left < left)
            {
                //float off = 0f;
                //if((int)cam2d.Position.X != (int)cam2d.BoundingRectangle.Left)
                //    off = cam2d.Position.X - cam2d.BoundingRectangle.Left;

                //if(off < 0)
                //    off = 0;

                //int x = (int)(left - cam2d.BoundingRectangle.Left + off);
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

            if(map.builder.b != null)
            {
                var hamSize = new Vector2(48);
                Builder.DrawHamBuild(sb, new Vector2(map.builder.b.Center.X - hamSize.X / 2, map.builder.b.CollisionBox.Top - hamSize.Y - 55), hamSize);
            }

            //sb.Draw(UtilityContent.box, cam2d.BoundingRectangle.ToRectangle(), new Color(Color.Red, 0.0005f));
            //sb.Draw(UtilityContent.box, new Rectangle(cam2d.Position.ToPoint(), new Point(32, 32)), Color.Blue);

            sb.End();


            //  SCREEN
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

            treeBar.Draw(sb);

            lbMoney.Draw(sb);

            var pos = new Vector2(GHelper.Center(mb.btTurret.Rectangle, lbTurret.Size + lbTurret.TextSize).X, mb.btTurret.Position.Y - lbTurret.Size.Y * 2);
            lbTurret.SetPosition(pos);
            lbTurret.Draw(sb);

            pos = new Vector2(GHelper.Center(mb.btSniper.Rectangle, lbSniper.Size + lbSniper.TextSize).X, mb.btSniper.Position.Y - lbSniper.Size.Y * 2);
            lbSniper.SetPosition(pos);
            lbSniper.Draw(sb);

            pos = new Vector2(GHelper.Center(mb.btCata.Rectangle, lbCata.Size + lbCata.TextSize).X, mb.btCata.Position.Y - lbCata.Size.Y * 2);
            lbCata.SetPosition(pos);
            lbCata.Draw(sb);


            map.DrawScreen(sb);

            i = 0;
            var mpos = map.MouseWorldPos();
            DrawString(sb, $"Mouse: X:{(int)mpos.X}  Y:{(int)mpos.Y}");
            DrawString(sb, $"PlayerCenter: X:{(int)map.player.Center.X}  Y:{(int)map.player.Center.Y}");
            DrawString(sb, $"Camera: X:{(int)cam2d.Position.X}  Y:{(int)cam2d.Position.Y}");
            DrawString(sb, $"Camera: W:{(int)cam2d.BoundingRectangle.Right}  H:{(int)cam2d.BoundingRectangle.Left}");

            if(muLerp > 0.01f)
                mu.Draw(sb, muLerp);

            mb.Draw(sb, lerpTimer);

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
