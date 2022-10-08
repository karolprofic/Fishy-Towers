using StateMachine.GameStateMachine.Params;

namespace StateMachine.GameStateMachine.States
{
	public class Menu : State<EState>
	{
		private MenuParams _params;
		public Menu(StateMachine<EState> stateMachine, StateParams @params = null) : base(stateMachine, @params)
		{
			_params = (MenuParams) @params;
		}

		public override void Initiate()
		{
			var optionBox = new FunctionsBox();
			States.Add(stateMachine[EState.Menu],optionBox);
		}
		protected override void OnStart() { }
		protected override void OnEnd() { }
	}	
}
