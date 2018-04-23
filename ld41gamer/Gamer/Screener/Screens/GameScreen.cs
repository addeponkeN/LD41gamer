using ld41gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ld41gamer.Gamer.StateMachine;
using ld41gamer.Gamer.StateMachine.GameStates;
using Obo.GameUtility;
using Microsoft.Xna.Framework.Input;
using System;

namespace ld41gamer.Gamer.Screener
{
    public class GameScreen : Screen
    {

        public StateManager StateManager { get; set; }

        public GameScreen()
        {
            StateManager = new StateManager();

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0);
        }

        public override void Load()
        {
            base.Load();
            if(Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            GameContent.Load(Content, ScreenManager.GraphicsDevice);
            // SoundManager.Load(Content);


            AddState(new GameStatePlaying(this));
        }

        public override void LoadAfterLoad()
        {
            base.LoadAfterLoad();
            SoundManager.LoopSound(GameSoundType.Song2, SoundManager.Music);

        }

        public override void ReadAnswer(PopupAnswer ok)
        {
            base.ReadAnswer(ok);
            switch(ok)
            {
                case PopupAnswer.Yes:
                    ExitScreen(new MainMenuScreen());
                    MBMan.list.Clear();
                    break;
                case PopupAnswer.No:
                    break;
                case PopupAnswer.None:
                    break;
                case PopupAnswer.Ok:
                    ExitScreen(new MainMenuScreen());
                    MBMan.list.Clear();
                    break;
                case PopupAnswer.Cancel:
                    break;
            }
        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

            if(Input.KeyClick(Keys.Escape))
            {
                SoundManager.StopLoop(GameSoundType.TowerBuilding);
                AddPopupScreen(new PopupScreen("Exit to Menu?", GameContent.font14, GameContent.bigplank, GameContent.btplank, new Color(200, 200, 200), 220, 145, GameContent.font24, PopupType.YesNo), true);
            }

        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

            StateManager.Update(gt, this);



            if(Input.KeyClick(Keys.NumPad1))
                SoundManager.PlayTowerHit();
            if(Input.KeyClick(Keys.NumPad2))
                SoundManager.PlayEnemyHit();
        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            base.Draw(sb, gt);

            foreach(GameState state in StateManager.states)
            {
                state.Draw(sb, null);
            }

            DrawChildScreens(sb, gt);
            DrawPopups(sb, gt);
        }

        public void AddState(GameState state)
        {
            state.Load(Content);
            state.Init();
            StateManager.AddState(state);
        }

    }
}
