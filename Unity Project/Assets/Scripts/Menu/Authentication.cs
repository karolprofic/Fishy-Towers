using ManagersSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Authentication : MonoBehaviour
{
    //public/inspector
    [SerializeField] private TMP_Text loginStatus;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button logInButton;
    [SerializeField] private Button logOutButton;

    //unity events
    private void Start()
    {
        Button button = logInButton.GetComponent<Button>();
        button.onClick.AddListener(() => {
            Managers.Firebase.LogInOrSignIn(emailInput.text, passwordInput.text);
        });
        
        button = logOutButton.GetComponent<Button>();
        button.onClick.AddListener(() => {
            Managers.Firebase.SignOut();
        });
    }

    private void FixedUpdate()
    {
        if (Managers.Firebase.status == null)
        {
            return;
        }
        loginStatus.text = Managers.Firebase.status;
    }
}
