namespace StateMachine.GameStateMachine.Params
{
	public class GameStateMachineParams : StateParams
	{
		public LoadingScreenParams LoadingScreenParams;
		public MenuParams MenuParams;
		public OptionParams OptionParams;

		public GameStateMachineParams(
			LoadingScreenParams loadingScreenParams,
			MenuParams menuParams,
			OptionParams optionParams)
		{
			LoadingScreenParams = loadingScreenParams;
			MenuParams = menuParams;
			OptionParams = optionParams;
		}
		
	}	
}