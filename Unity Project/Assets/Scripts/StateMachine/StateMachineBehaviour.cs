using System;
using UnityEngine;

namespace StateMachine
{
    [Serializable]
    public abstract class StateMachineBehaviour<T, TY> : MonoBehaviour where T : Enum where TY : StateMachine<T>, new()
    {
        protected TY stateMachine = new TY();
        protected StateParams _params;
        protected T defaultState;
        public State<T> CurrentState => stateMachine.CurrenState;
        
        public abstract void Run(string stateName);
        protected abstract void ParamsInitialize();
        protected abstract void LateStart();
        
        public bool Run(T t)
        {
            return stateMachine.Run(t);
        }

        public void Run(int i)
        {
            stateMachine.Run((T) (object)i);
        }
        
        private void Start()
        {
            ParamsInitialize();
            stateMachine.Initialize(_params);
            stateMachine.LoadDependencies();
            stateMachine.ForceState(defaultState);
            LateStart();
        }
    }
}
