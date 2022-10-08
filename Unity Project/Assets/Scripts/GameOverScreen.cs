using TMPro;
using UnityEngine;
using System.Linq;

public class GameOverScreen : EndScreen
{
	[SerializeField] private TextMeshProUGUI gainedSeaweed;
	[SerializeField] private TextMeshProUGUI destoryedEnemies;

	// public override void Open()
	// {
	// 	base.Open();
	// 	// Statistic gainedSeaweedStat = ManagersSpace.Managers.Statistics.statistics.Array.Where(a => a.Name.Equals("gainedSeaweed")).FirstOrDefault();
	// 	// Statistic destoryedEnemiesStat = ManagersSpace.Managers.Statistics.statistics.Array.Where(a => a.Name.Equals("defeatedEnemies")).FirstOrDefault();
	// 	// gainedSeaweed.text = gainedSeaweedStat.Value.ToString();
	// 	// destoryedEnemies.text = destoryedEnemiesStat.Value.ToString();
	// }
}