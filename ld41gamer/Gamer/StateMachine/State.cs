using Microsoft.Xna.Framework;
using ld41gamer.Gamer.Screener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer.StateMachine
{
    public class State
    {
        public StateManager StateManager { get; set; }

        public List<State> ChildStates = new List<State>();

        bool initialized;

        public bool IsExiting;

        public State() { }

        public virtual void Init()
        {
            initialized = true;
        }

        public virtual void Update(GameTime gt, GameScreen gs)
        {
            if(!initialized)
                Init();

            for(int i = 0; i < ChildStates.Count; i++)
            {
                ChildStates[i].Update(gt, gs);
                if(ChildStates[i].IsExiting)
                    ChildStates.RemoveAt(i);
            }

        }

        public virtual void ExitState()
        {
            IsExiting = true;
        }
    }
}
