using ManagersSpace;
using UnityEngine;
using UnityEngine.UI;

public class TestAddValue : MonoBehaviour
{   
    [SerializeField]
    private Button addValueButton;
    
    [SerializeField]
    private string statisticName;
    
    [SerializeField]
    private int statisticValue;
    
    void Start () {
        Button button = addValueButton.GetComponent<Button>();
        button.onClick.AddListener(AddValueToStatistic);
    }
    void AddValueToStatistic()
    {
        Managers.Statistics.IncreaseStatisticValue(statisticName, statisticValue);
    }
    
}
