using System.IO;
using UnityEngine;

namespace ManagersSpace
{
	public class StatisticsManager : Manager
	{ 
		//public/inspector
		public Statistics statistics = new Statistics();
		
		//unity methods
		private void OnEnable()
		{
			LoadData();
		}
		
		public void Reset()
		{
			foreach (var statistic in statistics.Array)
			{
				statistic.Value = 0;
			}
		}
		
		//public methods
		public void SaveData()
		{
			var textStatistics = JsonUtility.ToJson(statistics, true);
			File.WriteAllText(Application.dataPath + "/Resources/JSON/Statistics.json", textStatistics);
		}
		
		public int GetStatisticValue(string statisticName)
		{
			foreach (var statistic in statistics.Array)
			{
				if (statistic.Name == statisticName)
				{
					return statistic.Value;
				}
			}
			return 0;
		}
		public void IncreaseStatisticValue(string statisticName, int statisticValue)
		{
			foreach (var statistic in statistics.Array)
			{
				if (statistic.Name == statisticName)
				{
					statistic.Value += statisticValue;
					break;
				}
			}
		}
		
		public void DecreaseStatisticValue(string statisticName, int statisticValue)
		{
			foreach (var statistic in statistics.Array)
			{
				if (statistic.Name == statisticName)
				{
					statistic.Value -= statisticValue;
					break;
				}
			}
		}
		
		public int GetScore()
		{
			int score = 0;
			foreach (var statistic in statistics.Array)
			{
				score += statistic.Value;
			}
			return score;
		}
		
		//private methods
		private void LoadData()
		{
			var jasonStatistics = Resources.Load<TextAsset>("JSON/Statistics");
			statistics = JsonUtility.FromJson<Statistics>(jasonStatistics.text);
		}
	}
}