using ManagersSpace;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Purchasing;
using Product = UnityEngine.Purchasing.Product;

public class IAPStore : MonoBehaviour
{
    //public
    [SerializeField] private TMP_Text infoText;
    
    //unity methods
    public void Start()
    {
        SetInfoBoxText();
    }

    //private methods
    private int GetProductValue(Product product)
    {
        return product.definition.id switch
        {
            //Todo: const string na początku klasy
            "com.studentsproject.babysharks.coin250" => 250,
            "com.studentsproject.babysharks.coin500" => 500,
            "com.studentsproject.babysharks.coin1000" => 1000,
            _ => 0
        };
    }

    private void SetInfoBoxText()
    {
        var titleBeginning = LocalizationSettings.StringDatabase.GetLocalizedString("Store", "TITLE_BEGINNING");
        var titleEnding = LocalizationSettings.StringDatabase.GetLocalizedString("Store", "TITLE_ENDING");
        infoText.text = titleBeginning + Managers.Statistics.GetStatisticValue("premiumCurrency").ToString() + titleEnding;
    }
    
    //public methods
    public void OnPurchaseComplete(Product product)
    {
        Managers.Statistics.IncreaseStatisticValue("premiumCurrency", GetProductValue(product));
        Managers.Statistics.IncreaseStatisticValue("openBoxes", 1);
        Managers.Achievements.SearchForCompletedAchievements();
        Managers.Statistics.SaveData();
        Managers.Achievements.SaveData();
        Managers.Firebase.StartSynchronization();
        SetInfoBoxText();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        infoText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Store", "ERROR");
    }
}
