using System.Collections.Generic;
using Units;
using UnityEngine;
using System.Linq;

public class UnitsChoosingBoard : MonoBehaviour
{
	[SerializeField] private Transform elementsParent;
	[SerializeField] private GameObject elementPrefab;

	private List<UnitsChoosingBoardElement> elements = new List<UnitsChoosingBoardElement>();

	public void CreateCarouselle(List<Unit.Type> units)
	{
		ClearElements();

		foreach(Unit.Type unit in units)
			AddUnit(unit);
	}

	private void AddUnit(Unit.Type unit)
	{
		if(unit == Unit.Type.None) return;
		elements.Add(Instantiate(elementPrefab, elementsParent).GetComponent<UnitsChoosingBoardElement>());
		elements.Last().Initialize(unit);
	}

	private void ClearElements()
	{
		foreach(UnitsChoosingBoardElement element in elements)
			Destroy(element.gameObject);
		elements.Clear();
	}
}