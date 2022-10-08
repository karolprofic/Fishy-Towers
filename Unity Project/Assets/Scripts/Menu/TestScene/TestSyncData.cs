using ManagersSpace;
using UnityEngine;
using UnityEngine.UI;

public class TestSyncData : MonoBehaviour
{
    [SerializeField]
    private Button syncButton;
    
    void Start () {
        Button sync = syncButton.GetComponent<Button>();
        sync.onClick.AddListener(() => {
            Managers.Firebase.StartSynchronization();
        });
    }
    
}
