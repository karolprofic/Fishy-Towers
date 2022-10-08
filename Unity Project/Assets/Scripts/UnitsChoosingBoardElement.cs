using Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using ManagersSpace;

public class UnitsChoosingBoardElement : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI costText;
	public static readonly UnityEvent<Unit.Type> OnPlayerSelectorClick = new();

	private Unit.Type unitType;

	public void Initialize(Unit.Type unitType_)
	{
		unitType = unitType_;
		image.sprite = ManagersSpace.Managers.Units.GetMiniature(unitType);
		costText.text = Managers.Units.UnitCost(unitType_).ToString();
	}

	public void OnClick()
	{
		OnPlayerSelectorClick.Invoke(unitType);
	}
}