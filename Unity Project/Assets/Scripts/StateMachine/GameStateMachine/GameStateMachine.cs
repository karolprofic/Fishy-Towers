using StateMachine.GameStateMachine.Params;
using StateMachine.GameStateMachine.States;

namespace StateMachine.GameStateMachine
{
	public class GameStateMachine : StateMachine<EState>
	{
		private GameStateMachineParams _params;

		public override void Initialize(StateParams @params)
		{
			_params = (GameStateMachineParams) @params;
			States.Add(EState.LoadingScreen, new LoadingScreen(this, _params.LoadingScreenParams));
			States.Add(EState.Menu, new Menu(this, _params.MenuParams));
			States.Add(EState.Option, new Option(this, _params.OptionParams));
		}
	}	
}
