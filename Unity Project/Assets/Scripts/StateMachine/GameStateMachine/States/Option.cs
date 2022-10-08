using StateMachine.GameStateMachine.Params;

namespace StateMachine.GameStateMachine.States
{
	public class Option : State<EState>
	{
		private OptionParams _params;
		public Option(StateMachine<EState> stateMachine, StateParams @params = null) : base(stateMachine, @params)
		{
			_params = (OptionParams) @params;
		}

		public override void Initiate() { }
		protected override void OnStart() { }
		protected override void OnEnd() { }
	}	
}
