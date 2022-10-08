using StateMachine.GameStateMachine.Params;

namespace StateMachine.GameStateMachine.States
{
	public class LoadingScreen : State<EState>
	{
		private LoadingScreenParams _params;
		public LoadingScreen(StateMachine<EState> stateMachine, StateParams @params = null) : base(stateMachine, @params)
		{
			_params = (LoadingScreenParams) @params;
		}

		public override void Initiate() { }
		protected override void OnStart() { }
		protected override void OnEnd() { }
	}
}