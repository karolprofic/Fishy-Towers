using System.IO;
using UnityEngine;

namespace ManagersSpace
{
	public class AchievementsManager : Manager
	{
		//public/inspector
		public Achievements achievements = new Achievements();

		//unity methods
		public void Reset()
		{
			foreach (var achievement in achievements.Array)
			{
				achievement.IsAchieved = false;
			}   
		}
		
		//public methods
		public void SaveData()
		{
			var textAchievements = JsonUtility.ToJson(achievements, true);
			File.WriteAllText(Application.dataPath + "/Resources/JSON/Achievements.json", textAchievements);
		}

		public void SearchForCompletedAchievements()
		{
			foreach (var achievement in achievements.Array)
			{
				var statisticValue = GetStatisticValue(achievement.StatisticToCheck);
				var conditionValue = achievement.ValueToAchieve;
            
				if (statisticValue >= conditionValue && achievement.IsAchieved == false)
				{
					achievement.IsAchieved = true;
                
					Managers.Statistics.IncreaseStatisticValue("allAchievements", 1);
					switch (achievement.Type)
					{
						//Todo: typy mogły by być jako enum
						case "BRONZE":
							//Todo: stringi raczej wyciągać jako const na początek klasy
							Managers.Statistics.IncreaseStatisticValue("bronzeAchievements", 1);
							break;
						case "SILVER":
							Managers.Statistics.IncreaseStatisticValue("silverAchievements", 1);
							break;
						case "GOLD":
							Managers.Statistics.IncreaseStatisticValue("goldAchievements", 1);
							break;
						case "PLATINUM":
							Managers.Statistics.IncreaseStatisticValue("platinumAchievements", 1);
							break;
					}
					
				}
            
			}
        
		}
		
		//private methods
		private void OnEnable()
		{
			LoadData();
		}
		
		private void LoadData()
		{
			var jasonAchievements = Resources.Load<TextAsset>("JSON/Achievements");
			achievements = JsonUtility.FromJson<Achievements>(jasonAchievements.text);
		}

		private int GetStatisticValue(string statisticName)
		{
			foreach (var statistic in Managers.Statistics.statistics.Array)
			{
				if (statistic.Name == statisticName)
				{
					return statistic.Value;
				}
			}

			return -1;
		}

	}
}