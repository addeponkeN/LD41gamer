using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Obo.GameUtility;
using Obo.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer.Screener
{
    public class MainMenuScreen : MenuScreen
    {

        Sprite title;
        Sprite bg;

        Camera2D cam;

        Button btPlay, btSettings, btCredits, btQuit;

        Button btPlus, btMinus;

        Label lbVol;

        float vol = 0f;

        bool exiting;

        public MainMenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(2);
            TransitionOffTime = TimeSpan.FromSeconds(1);
        }

        public override void Load()
        {
            base.Load();

            if(Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            GameContent.LoadMenu(Content);

            SoundManager.LoopSound(GameSoundType.Song2, vol);


            var port = new ScalingViewportAdapter(ScreenManager.GraphicsDevice, 1920, 1080);
            cam = new Camera2D(port);
            cam.MinimumZoom = 0.1f;
            cam.MaximumZoom = 3.1f;

            var gd = ScreenManager.GraphicsDevice;
            title = new Sprite(GameContent.title);
            title.Position = new Vector2(GHelper.Center(Globals.ScreenBox, title.Size).X, Globals.ScreenWidth / 35);
            //title.SetSize(Globals.ScreenWidth, Globals.ScreenHeight);


            bg = new Sprite(GameContent.bgmenu);
            //title.SetSize(Globals.ScreenWidth, Globals.ScreenHeight);

            var size = new Vector2(192, 64);

            btPlay = new Button(gd, GameContent.btplank, (int)size.X, (int)size.Y, "Play");
            btPlay.SetColor(new Color(230, 230, 230));
            btPlay.DrawOutline = false;
            btPlay.font = GameContent.font24;

            btSettings = new Button(gd, GameContent.btplank, (int)size.X, (int)size.Y, "Options");
            btSettings.SetColor(new Color(230, 230, 230));
            btSettings.DrawOutline = false;
            btSettings.font = GameContent.font24;

            btCredits = new Button(gd, GameContent.btplank, (int)size.X, (int)size.Y, "Credits");
            btCredits.SetColor(new Color(230, 230, 230));
            btCredits.DrawOutline = false;
            btCredits.font = GameContent.font24;

            btQuit = new Button(gd, GameContent.btplank, (int)size.X, (int)size.Y, "Quit");
            btQuit.SetColor(new Color(230, 230, 230));
            btQuit.DrawOutline = false;
            btQuit.font = GameContent.font24;

            //vbVol = new ValueButton(gd, Vector2.Zero, "Volume", new string[] { "-", "+" });

            btMinus = new Button(gd, GameContent.btplank, 32, 32, "-");
            btMinus.SetColor(new Color(230, 230, 230));
            btMinus.DrawOutline = false;
            btMinus.font = GameContent.font14;

            btPlus = new Button(gd, GameContent.btplank, 32, 32, "+");
            btPlus.SetColor(new Color(230, 230, 230));
            btPlus.DrawOutline = false;
            btPlus.font = GameContent.font14;

            lbVol = new Label(GameContent.font14, "Volume");

            AddComponent(btPlay);
            //AddComponent(btSettings);
            AddComponent(btCredits);
            //AddComponent(vbVol);
            AddComponent(btQuit);

            Vector2 start = new Vector2(GHelper.Center(title.Rectangle, btPlay.Size).X, title.Rectangle.Bottom - btPlay.Size.Y);
            for(int i = 0; i < Components.Count; i++)
            {
                var c = Components[i];
                c.Position = start + new Vector2(0, (i * size.Y) + (i * 32));
            }
        }

        public override void LoadAfterLoad()
        {
            base.LoadAfterLoad();
        }

        public override void ReadAnswer(PopupAnswer ok)
        {
            base.ReadAnswer(ok);
            switch(ok)
            {
                case PopupAnswer.None:
                    break;
                case PopupAnswer.Ok:
                    break;
                case PopupAnswer.Cancel:
                    break;
                case PopupAnswer.Yes:
                    break;
                case PopupAnswer.No:
                    break;
            }
        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

            var dt = gt.Delta();
            lbVol.Text = "Volume " + (int)(SoundManager.Master * 100);

            if(vol < .5f)
            {
                vol += dt * 0.5f;
                SoundManager.SetVol(GameSoundType.Song2, vol);
            }

            if(exiting)
            {
                vol -= dt * 2.0f;
                SoundManager.SetVol(GameSoundType.Song2, vol);

                if(vol <= 0.25f)
                    SoundManager.StopLoop(GameSoundType.Song2);

            }


        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);


            if(btPlus.IsClicked)
            {
                SoundManager.Master = MathHelper.Clamp(SoundManager.Master += (float)0.1, 0, 1f);
                SoundManager.SetVol(GameSoundType.Song2, vol);
            }

            if(btMinus.IsClicked)
            {
                SoundManager.Master = MathHelper.Clamp(SoundManager.Master -= (float)0.1, 0, 1f);
                SoundManager.SetVol(GameSoundType.Song2, vol);
            }


            if(btPlay.IsTriggered)
            {
                exiting = true;
                //SoundManager.StopLoop(GameSoundType.Song2);
                ExitScreen(new GameScreen());
            }

            if(btCredits.IsTriggered)
            {
                AddPopupScreen(new PopupScreen(
                    "          CREDITS\n" +
                    "ponker, wwh, johnstapler\n" +
                    "  Made for LudumDare41", GameContent.font24, GameContent.bigplank, GameContent.btplank, new Color(200, 200, 200), 350, 150, GameContent.font24, PopupType.Ok), true);
            }

            if(btQuit.IsTriggered)
            {
                Game1.ExitGame();
            }

        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam.GetViewMatrix());

            bg.Draw(sb);

            btMinus.Position = btQuit.Position + new Vector2(0, btQuit.Size.Y + 24);
            btPlus.Position = btQuit.Position + new Vector2(btQuit.Size.X - btPlus.Size.X, btQuit.Size.Y + 24);
            lbVol.Position = new Vector2(GHelper.Center(btQuit.Rectangle, lbVol.TextSize).X, GHelper.Center(btMinus.Rectangle, lbVol.Size).Y);

            title.Draw(sb);
            btPlus.Draw(sb);
            btMinus.Draw(sb);

            lbVol.Draw(sb);

            sb.End();

            base.Draw(sb, gt);




        }

    }
}
