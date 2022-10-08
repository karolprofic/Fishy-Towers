using ManagersSpace;
using UnityEngine;
using UnityEngine.UI;

public class TestSaveData : MonoBehaviour
{
    [SerializeField]
    private Button saveButton;
    
    void Start () {
        Button save = saveButton.GetComponent<Button>();
        save.onClick.AddListener(SaveDataFiles); ;
    }
    void SaveDataFiles()
    {
        Managers.Statistics.SaveData();
        Managers.Achievements.SaveData();
    }
    
}
