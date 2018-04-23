using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using ld41gamer.Gamer.Screener;
using ld41gamer.Screener;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ld41gamer
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        static bool exitGame;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Globals.ProjectName = typeof(Game1).Namespace;

            graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            graphics.PreferredBackBufferHeight = Globals.ScreenHeight;

            OboGlobals.Load(Globals.ScreenWidth, Globals.ScreenHeight);

            Window.Title = Globals.ProjectName;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;


            Window.IsBorderless = false;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            IsFixedTimeStep = true;
            IsMouseVisible = true;

            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 240);

            var screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new MainMenuScreen());

        }


        public static void ExitGame(List<Action> actions = null)
        {
            exitGame = true;

            if(actions != null)
            {
                for(int i = 0; i < actions.Count; i++)
                    actions[i].Invoke();
                Thread.Sleep(50);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(exitGame)
                Exit();
        }

    }
}
