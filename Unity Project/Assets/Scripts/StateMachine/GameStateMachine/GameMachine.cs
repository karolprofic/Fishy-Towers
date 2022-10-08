using StateMachine.GameStateMachine.Params;

namespace StateMachine.GameStateMachine
{
	public class GameMachine : StateMachineBehaviour<EState, GameStateMachine>
	{
		public static GameMachine Instance => _instance;
		private static GameMachine _instance;

		private void Awake()
		{
			this.OnlyOne(ref _instance);
		}

		public override void Run(string stateName)
		{
			EState state = stateName switch
			{
				"LoadingScreen" => EState.LoadingScreen,
				"Menu" => EState.Menu,
				"Option" => EState.Option,
				_ => EState.Error
			};
			stateMachine.Run(state);
		}

		protected override void ParamsInitialize()
		{
			_params = new GameStateMachineParams(
				new LoadingScreenParams(),
				new MenuParams(),
				new OptionParams());

			defaultState = EState.Menu;
		}

		protected override void LateStart() {}
	}
}