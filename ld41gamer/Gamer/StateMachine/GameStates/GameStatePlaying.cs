﻿using System;
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
        Level1_0,

        Level2_20,

        Level3_50,

        Level4_90,

        Level5_150,

        Level6_210,

        Level7_300,

        Level8_420,

        Level9_550,

        Level10_670,
        Level11_740,
        Level8_500_Break,
        Level9_630_break,
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
        Label lbTime;

        public MenuUpgrade mu;
        public MenuBuy mb;

        public TreeHp treeBar;

        float muLerp;

        public bool AnyUiHovered => mu.buttons.Any(x => x.IsHovered);

        public TimeSpan time;

        public GameStatePlaying(GameScreen gs) : base(gs)
        {
            FadeInTime = 2f;
            FadeOutTime = 2f;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

            TreeBranch.Cost = 30;
            Turret.TurretCost = 20;
            Turret.SniperCost = 40;
            Turret.CataCost = 100;
            Upgrades.TreeBranches = 0;
            Upgrades.Player_BuildTime = 1f;
            Enemy.HpIncreaser = 0;

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
                    if(!map.player.IsShopping)
                        return;

                    if(!Upgrades.TreeBranchesMaxed)
                        map.player.IsShoppingBranch = !map.player.IsShoppingBranch;
                });

            //mu.AddButton(UBBaseType.BuildSpeed, () =>
            //{
            //    if(!Upgrades.Player_BuildTimeMaxed)
            //        map.player.BuildSpeed += 0.5f;
            //});


            var pos = mb.btTurret.Position - new Vector2(0, 14);
            lbTurret = new GameLabel(GameContent.acorn, "20", pos, GameContent.font14);
            lbTurret.Item.SetSize(32);

            pos = mb.btTurret.Position - new Vector2(0, 14);
            lbSniper = new GameLabel(GameContent.acorn, "40", pos, GameContent.font14);
            lbSniper.Item.SetSize(32);

            pos = mb.btTurret.Position - new Vector2(0, 14);
            lbCata = new GameLabel(GameContent.acorn, "100", pos, GameContent.font14);
            lbCata.Item.SetSize(32);

            lbTime = new Label(GameContent.font48, "00:00");
            lbTime.Position = new Vector2(GHelper.Center(Globals.ScreenBox, lbTime.TextSize).X, 8);


            treeBar = new TreeHp();
            treeBar.Posser(new Vector2(8, 8));

            lbMoney.Text = map.player.Money.ToString();
        }

        public Vector2 camDest;

        //  update always
        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            mu.Update(gt, this);

            map.Update(gt, game);

            time = TimeSpan.FromSeconds(map.GameTimer);
            lbTime.Text = time.ToString("mm':'ss");
            lbTime.Position = new Vector2(GHelper.Center(Globals.ScreenBox, lbTime.TextSize).X, 8);

            foreach(var bb in mu.buttons)
            {
                switch(bb.Type)
                {

                    case UBBaseType.Branch:
                        bb.Set(TreeBranch.Cost);
                        break;

                    case UBBaseType.BuildSpeed:
                        //bb.Set();
                        break;

                }
            }


            lbMoney.Text = map.player.Money.ToString();

            treeBar.Update(map.tree.HealthPoints, map.tree.MaxHealthPoints);

            MBMan.Update(gt);

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

            //if(Input.WheelDown)
            //{
            //    cam2d.ZoomOut(0.01f);
            //}
            //else if(Input.WheelUp)
            //{
            //    cam2d.ZoomIn(0.01f);
            //}

            //Input.ScrollValueOld = Input.ScrollValue;

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

            MBMan.Draw(sb);

            var pos = new Vector2(GHelper.Center(mb.btTurret.Rectangle, lbTurret.Size + lbTurret.TextSize).X, mb.btTurret.Position.Y - lbTurret.Size.Y * 2);
            lbTurret.SetPosition(pos);
            lbTurret.Draw(sb);

            pos = new Vector2(GHelper.Center(mb.btSniper.Rectangle, lbSniper.Size + lbSniper.TextSize).X, mb.btSniper.Position.Y - lbSniper.Size.Y * 2);
            lbSniper.SetPosition(pos);
            lbSniper.Draw(sb);

            pos = new Vector2(GHelper.Center(mb.btCata.Rectangle, lbCata.Size + lbCata.TextSize).X, mb.btCata.Position.Y - lbCata.Size.Y * 2);
            lbCata.SetPosition(pos);
            lbCata.Draw(sb);

            lbTime.Draw(sb);


            map.DrawScreen(sb);

            //i = 0;
            //var mpos = map.MouseWorldPos();
            //DrawString(sb, $"Mouse: X:{(int)mpos.X}  Y:{(int)mpos.Y}");
            //DrawString(sb, $"PlayerCenter: X:{(int)map.player.Center.X}  Y:{(int)map.player.Center.Y}");
            //DrawString(sb, $"Camera: X:{(int)cam2d.Position.X}  Y:{(int)cam2d.Position.Y}");
            //DrawString(sb, $"Camera: W:{(int)cam2d.BoundingRectangle.Right}  H:{(int)cam2d.BoundingRectangle.Left}");
            //DrawString(sb, $"");
            //DrawString(sb, $"");
            //DrawString(sb, $"");

            //DrawString(sb, $"Time: {(int)map.GameTimer}");
            //DrawString(sb, $"Level: {map.GameState.ToString()}");
            //DrawString(sb, $"Wormhole: {(int)map.holeSpawnTimer} / {(int)map.holeSpawnCd}");
            //DrawString(sb, $"Beaver: {(int)map.beavSpawnTimer} / {(int)map.beavSpawnCd}");
            //DrawString(sb, $"Ticks: {(int)map.spawnTicks}");


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
