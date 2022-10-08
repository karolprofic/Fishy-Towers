using ManagersSpace;
using UnityEngine;
using UnityEngine.UI;

public class TestResetData : MonoBehaviour
{
    [SerializeField]
    public Button resetButton;
    
    void Start () {
        Button reset = resetButton.GetComponent<Button>();
        reset.onClick.AddListener(ResetData);
    }
    
    void ResetData()
    {
        Managers.Statistics.Reset();
        Managers.Achievements.Reset();
    }
    
}
