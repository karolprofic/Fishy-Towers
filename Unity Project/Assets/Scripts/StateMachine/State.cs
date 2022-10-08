using System;
using System.Collections.Generic;

namespace StateMachine
{
    [Serializable]
    public abstract class State<T> where T : Enum
    {
        //protected
        protected readonly Dictionary<State<T>, FunctionsBox> States = new Dictionary<State<T>, FunctionsBox>();
        protected readonly StateMachine<T> stateMachine;

        //private methods
        private void Start(State<T> state)
        {
            OnStart();
            if (States.TryGetValue(state, out var box))
                box.OnStart?.Invoke();
        }
        
        //protected methods
        protected abstract void OnStart();
        protected abstract void OnEnd();

        protected State(StateMachine<T> stateMachine, StateParams @params = null)
        {
            this.stateMachine = stateMachine;
        }
        
        protected struct FunctionsBox
        {
            public Func<bool> Conditions;
            public Action OnStart;
            public Action OnEnd;

            public FunctionsBox(Func<bool> conditions = null, Action onStart = null, Action onEnd = null)
            {
                Conditions = conditions;
                OnStart = onStart;
                OnEnd = onEnd;
            }
        }
        
        //public methods
        public abstract void Initiate();

        public void ForceStart()
        {
            OnStart();
        }
        
        public void ForceStart(State<T> state)
        {
            Start(state);
        }
        
        public bool Switch(State<T> state)
        {
            if (!States.TryGetValue(state, out var box))
                return false;

            if (box.Conditions != null
                && !box.Conditions.Invoke())
                return false;
        
            box.OnEnd?.Invoke();
            OnEnd();
            state.Start(this);
            return true;
        }
    }
}