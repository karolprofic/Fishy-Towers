using System;
using System.Collections.Generic;

namespace StateMachine
{
    [Serializable]
    public abstract class StateMachine<T> where T : Enum
    {
        protected readonly Dictionary<T, State<T>> States = new Dictionary<T, State<T>>();
        public State<T> CurrenState { get; private set; }
        public State<T> this[T t] => States[t];

        public abstract void Initialize(StateParams @params);

        public void LoadDependencies()
        {
            States.ForEach(s => s.Value.Initiate());
        }

        public void ForceState(T estate)
        {
            if (!States.TryGetValue(estate, out var state)) 
                return;
            state.ForceStart();
            CurrenState = state;
        }
        
        public bool Run(T t)
        {
            var state = States[t];

            if (!CurrenState.Switch(state))
                return false;

            CurrenState = state;
            return true;
        }
    }
}