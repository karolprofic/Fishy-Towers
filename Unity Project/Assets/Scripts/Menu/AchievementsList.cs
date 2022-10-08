using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using ManagersSpace;

public class AchievementsList : MonoBehaviour
{
    //public/inspector
    [SerializeField]
    private GameObject achievementPrefab;
    
    [SerializeField]
    private Scrollbar scrollRect;
    
    [SerializeField]
    private Transform contentArea;

    //unity methods
    private void Start()
    {
        AddAchievementsToContentArea();
        ScrollContentAreaToTheTop();
    }
    
    //private methods
    private void AddAchievementsToContentArea()
    {
        foreach (var achievement in Managers.Achievements.achievements.Array)
        {
            var newPrefabInstance = Instantiate(achievementPrefab, contentArea);
            AddTextToPrefab(newPrefabInstance, achievement);
            AddIconToPrefab(newPrefabInstance, achievement);
            AddCheckMarkToPrefab(newPrefabInstance, achievement);
        }
    }
    
    private void ScrollContentAreaToTheTop()
    {
        contentArea.GetComponent<RectTransform>().localPosition = new Vector3(0,-8000,0);
        scrollRect.value = 1f;
    }
    
    //static methods
    private static void AddTextToPrefab(GameObject prefab, Achievement achievement)
    {
        var leftText = prefab.transform.GetChild(0).GetComponent<TMP_Text>();
        var rightText = prefab.transform.GetChild(1).GetComponent<TMP_Text>();
        var nameTableEntryName = achievement.Name;
        var descriptionTableEntryName = achievement.Description;
        var achievementName = LocalizationSettings.StringDatabase.GetLocalizedString("Achievements", nameTableEntryName);
        var achievementDescription = LocalizationSettings.StringDatabase.GetLocalizedString("Achievements", descriptionTableEntryName);
        leftText.text = achievementName;
        rightText.text = achievementDescription;
    }
    
    private static void AddIconToPrefab(GameObject prefab, Achievement achievement)
    {
        var achievementIcon = prefab.transform.GetChild(2).GetComponent<Image>();
        var iconTexture = Resources.Load<Texture2D>("Icons/" + achievement.ImageSource);
        var iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, 200, 200), new Vector2(0, 0));
        achievementIcon.sprite = iconSprite;
    }

    private static void AddCheckMarkToPrefab(GameObject prefab, Achievement achievement)
    {
        var isAchieveIcon = prefab.transform.GetChild(3).GetComponent<Image>();
        if (!achievement.IsAchieved) 
            return;
        var markTexture = Resources.Load<Texture2D>("Icons/CheckMark");
        var markSprite = Sprite.Create(markTexture, new Rect(0, 0, 160, 160), new Vector2(0, 0));
        isAchieveIcon.sprite = markSprite;
    }
}
