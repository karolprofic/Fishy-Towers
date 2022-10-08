using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField] protected CanvasGroup canvasGroup;
	[SerializeField] protected LevelLoader levelLoader;

	//public methods
	public virtual void Open()
	{
		canvasGroup.Enable();
	}

	public virtual void Close()
	{
		canvasGroup.Disable();
	}

	public virtual void ExitGame_()
	{
		levelLoader.LoadLevel("MainMenu");
	}
}