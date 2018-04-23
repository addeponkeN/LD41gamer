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

            SoundManager.LoopSound(GameSoundType.Song1, vol);


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

            btPlay = new Button(gd, size, "Play");
            btSettings = new Button(gd, size, "Options");
            btCredits = new Button(gd, size, "Credits");
            btQuit = new Button(gd, size, "Quit");

            AddComponent(btPlay);
            AddComponent(btSettings);
            AddComponent(btCredits);
            AddComponent(btQuit);

            Vector2 start = new Vector2(GHelper.Center(title.Rectangle, btPlay.Size).X, title.Rectangle.Bottom - btPlay.Size.Y);
            for(int i = 0; i < Components.Count; i++)
            {
                var c = Components[i];
                c.Position = start + new Vector2(0, (i * size.Y) + (i * 16));
            }

        }

        public override void LoadAfterLoad()
        {
            base.LoadAfterLoad();
        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

            var dt = gt.Delta();

            if(vol < .5f)
            {
                vol += dt * 0.5f;
                SoundManager.SetVol(GameSoundType.Song1, vol);
            }

            if(btPlay.IsTriggered)
            {
                exiting = true;
                ExitScreen(new GameScreen());
            }

            if(exiting)
            {
                vol -= dt * 2.0f;
                SoundManager.SetVol(GameSoundType.Song1, vol);

                if(vol <= 0.05f)
                    SoundManager.StopLoop(GameSoundType.Song1);

            }


        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);
        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, cam.GetViewMatrix());

            bg.Draw(sb);
            title.Draw(sb);


            sb.End();

            base.Draw(sb, gt);

        }

    }
}
