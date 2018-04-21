using ld41gamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ld41gamer.Gamer.StateMachine;
using ld41gamer.Gamer.StateMachine.GameStates;

namespace ld41gamer.Gamer.Screener
{
    public class GameScreen : Screen
    {

        public StateManager StateManager { get; set; }

        public GameScreen()
        {
            StateManager = new StateManager();
        }

        public override void Load()
        {
            base.Load();
            if(Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            GameContent.Load(Content);

            AddState(new GameStatePlaying(this));
        }

        public override void LoadAfterLoad()
        {
            base.LoadAfterLoad();

        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

            StateManager.Update(gt, this);
        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

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
