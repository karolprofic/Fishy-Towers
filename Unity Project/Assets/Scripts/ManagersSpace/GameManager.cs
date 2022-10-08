using UnityEngine;

namespace ManagersSpace
{
	public class GameManager : Manager
	{
		//public/inspector
		public readonly Vector3 storageMapLocation = new(100.0f, 100.0f, 100.0f);
		public GameOverScreen gameOverScreen;

		//in barrier
		public void GameOver()
		{
			Managers.Battle.OnLose.Invoke();
			gameOverScreen.Open();
		}
	}
}