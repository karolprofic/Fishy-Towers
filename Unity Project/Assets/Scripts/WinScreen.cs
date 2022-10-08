using UnityEngine;

public class WinScreen : EndScreen
{
	public void NextLevel_()
	{
		levelLoader.LoadLevel("Game");
	}
}