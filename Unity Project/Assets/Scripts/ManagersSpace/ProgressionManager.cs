using UnityEngine;
using TMPro;

namespace ManagersSpace
{
	public class ProgressionManager : Manager
	{
		//public/inspector
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private ushort baseLvlsPerStage;
		[SerializeField] private ushort lvlsAdderPerStage;

		public ushort Stage;
		public ushort Lvl;

		//unity methods

		private void Start()
		{
			LoadProgressFromPlayerPrefs();
			if(Stage <= 0) Stage = 1;
			if(Lvl <= 0) Lvl = 1;
			if(text!=null)text.text = $"Stage {Stage}-{Lvl}";
		}

		//public methods
		public ushort getHugeWavesNeededToWin()
		{
			return Lvl;
		}

		public void OnLose()
		{
			Stage = 1;
			Lvl = 1;
			SaveProgressFromPlayerPrefs();
		}

		public void OnWon()
		{
			if(++Lvl >= baseLvlsPerStage + lvlsAdderPerStage * Stage)
			{
				Stage++;
				Lvl = 1;
			}
			SaveProgressFromPlayerPrefs();
		}

		private void LoadProgressFromPlayerPrefs()
		{
			Stage = (ushort)PlayerPrefs.GetInt("stage");
			Lvl = (ushort)PlayerPrefs.GetInt("level");
		}

		private void SaveProgressFromPlayerPrefs()
		{
			PlayerPrefsExt.SavePlayerPrefs("stage", Stage);
			PlayerPrefsExt.SavePlayerPrefs("level", Lvl);
		}
	}
}