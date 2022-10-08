using UnityEngine;
using ManagersSpace;

public class Pause : MonoBehaviour
{
	//public/inspector
	[SerializeField] private CanvasGroup pauseGroup;
	[SerializeField] private LevelLoader levelLoader;

	//public methods
	public void OpenPauseScreen()
	{
		Managers.Battle.OnPause.Invoke();
		pauseGroup.Enable();
	}

	public void ClosePauseScreen()
	{
		Managers.Battle.OnUnPause.Invoke();
		pauseGroup.Disable();
	}

	public void ResumeGame_()
	{
		ClosePauseScreen();
	}

	public void ExitGame_()
	{
		levelLoader.LoadLevel("MainMenu");
	}
	
}