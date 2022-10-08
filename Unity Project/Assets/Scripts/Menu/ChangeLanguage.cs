using ManagersSpace;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    //public/inspector
    [SerializeField] private Button polishButton;
    [SerializeField] private Button englishButton;
    
    //unity methods
    void Start()
    {
        //Todo: Dlaczego jest getcomponent button od elementu, który jest buttonem?
        Button button = polishButton.GetComponent<Button>();
        polishButton.onClick.AddListener((() => {
            Managers.Settings.SetLanguage("pl");
        }));
        
        button = englishButton.GetComponent<Button>();
        englishButton.onClick.AddListener((() => {
            Managers.Settings.SetLanguage("en");
        }));
    }
    
}
