using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using ManagersSpace;

public class StatisticsList : MonoBehaviour
{
    //public/inspector
    [SerializeField] private GameObject statisticPrefab;
    [SerializeField] private Scrollbar scrollRect;
    [SerializeField] private Transform contentArea;
    
    //unity methods
    private void Start()
    {
        AddStatisticsToContentArea();
        ScrollContentAreaToTheTop();
    }
    
    //private methods
    private void AddStatisticsToContentArea()
    {        
        foreach (var statistic in Managers.Statistics.statistics.Array)
        {
            var newPrefabInstance = Instantiate(statisticPrefab, contentArea);
            var leftText = newPrefabInstance.transform.GetChild(0).GetComponent<TMP_Text>();
            var rightText = newPrefabInstance.transform.GetChild(1).GetComponent<TMP_Text>();
            
            var tableEntryName = statistic.Description;
            var nameOfStatistic = LocalizationSettings.StringDatabase.GetLocalizedString("Statistics", tableEntryName);
            
            leftText.text = nameOfStatistic;
            rightText.text = statistic.Value.ToString();
        }
    }
    
    private void ScrollContentAreaToTheTop()
    {
        contentArea.GetComponent<RectTransform>().localPosition = new Vector3(0,-4400,0);
        scrollRect.value = 1f;
    }
}
