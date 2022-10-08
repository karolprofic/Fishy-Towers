using ManagersSpace;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class StartUnitsDrawing : MonoBehaviour
{
	[SerializeField] private UnitsChoosingBoard unitsChoosingBoard;
	[SerializeField] private CanvasGroup startUnitsDrawingGroup;
	[SerializeField] private Transform elementsParent;
	[SerializeField] private GameObject elementPrefab;

	private List<GameObject> instantiated = new List<GameObject>();
	private List<Unit.Type> drawedUnits = new List<Unit.Type>();

	private void Start()
	{
		DrawUnits();
	}

	public void AcceptUnits_()
	{
		HideDrawWindow();
		Managers.Battle.StartGame();
	}

	public void RedrawUnits_()
	{
		Clear();

		DrawUnits();
	}

	private void DrawUnits()
	{
		var rnd = new System.Random();
		drawedUnits = Managers.Battle.startUnits.OrderBy(item => rnd.Next()).ToList();

		drawedUnits.Insert(0,Managers.Battle.startSeaweedGeneratorUnits[rnd.Next(0,Managers.Battle.startSeaweedGeneratorUnits.Count)]);


		for(int i = 0; i < Managers.Battle.howMuchToDraw; i++)
		{
			StartCoroutine(AddUnit(drawedUnits[i]));
		}
	}

	private IEnumerator AddUnit(Unit.Type unit)
	{
		if(unit == Unit.Type.None) yield break;
		GameObject temp = Instantiate(elementPrefab, elementsParent);
		instantiated.Add(temp);
		temp.GetComponentInChildren<ParticleSystem>().Play();
		yield return new WaitForSeconds(0.5f);
		temp.GetComponent<Image>().sprite = Managers.Units.GetMiniature(unit);
		yield return null;
	}

	private void HideDrawWindow()
	{
		unitsChoosingBoard.CreateCarouselle(drawedUnits.Take(Managers.Battle.howMuchToDraw).ToList());
		startUnitsDrawingGroup.Disable();
		Clear();
	}

	private void Clear()
	{
		foreach(GameObject obj in instantiated)
			Destroy(obj);
		instantiated.Clear();
	}
}