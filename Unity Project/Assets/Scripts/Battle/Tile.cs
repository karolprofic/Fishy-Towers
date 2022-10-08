using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unit = Units.Unit;

namespace Battle
{
	[Serializable]
	public class Tile : MonoBehaviour
	{
		//static
		public static readonly string Tag = "Tile";
		public static Tile selectedTile;

		//static events
		public static readonly UnityEvent<Tile> OnTileClick = new();

		//properties
		public bool UnitOnTile { get; set; }
		public bool HasAllies => Allies.Count > 0;
		public bool HasEnemies => Enemies.Count > 0;
		public int EnemiesAmount => Enemies.Count;
		public int AlliesAmount => Allies.Count;

		[SerializeField] public List<Unit> Allies = new();
		[SerializeField] public List<Unit> Enemies = new();
		[SerializeField] public Collider2D collider2D;

		//public Unit Ally { get; private set; }

		//public
		public int index;
		public Row row;
		public Transform placePosition;

		//events
		public readonly UnityEvent OnAllayChanged = new();

		public readonly UnityEvent<Unit> OnAllyEnter = new();
		public readonly UnityEvent<Unit> OnEnemyEnter = new();
		public readonly UnityEvent<Unit> OnAllyExit = new();
		public readonly UnityEvent<Unit> OnEnemyExit = new();

		//unity methods
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(selectedTile != null)
                    return;

                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (!collider2D.OverlapPoint(position))
                    return;
                
                selectedTile = this;
            }
        }

		//public methods
		public bool TrySetUnit(Unit unit)
		{
			if (unit == null || UnitOnTile)
				return false;
			if (unit.IsAlly)
			{
				UnitOnTile = true;
				unit.Tile = this;
			}
			unit.Row = row;
			OnAllayChanged.Invoke();
			return true;
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			Unit unit = other.gameObject.GetComponent<Unit>();

			if (other.gameObject.CompareTag(Unit.AllyTag))
			{
				Allies.Add(unit);
				OnAllyEnter.Invoke(unit);
			}
			else if (other.gameObject.CompareTag(Unit.EnemyTag))
			{
				Enemies.Add(unit);
				OnEnemyEnter.Invoke(unit);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			Unit unit = other.gameObject.GetComponent<Unit>();

			if (other.gameObject.CompareTag(Unit.AllyTag))
			{
				Allies.Remove(unit);
				OnAllyExit.Invoke(unit);
			}
			else if (other.gameObject.CompareTag(Unit.EnemyTag))
			{
				Enemies.Remove(unit);
				OnEnemyExit.Invoke(unit);
			}
		}

		//public bool TrySetUnit(Unit ally)
		//{
		//	if (Ally == null)
		//	{
		//		if (ally == null)
		//			return false;
		//		Ally = ally;
		//		Ally.row = row;
		//		OnAllayChanged.Invoke();
		//		return true;
		//	}
		//	//if (ally != null)
		//	//	return false;
		//	Ally = null;
		//	OnAllayChanged.Invoke();
		//	return true;
		//}

		//public void Remove() => Ally = null;
		// private void OnMouseUpAsButton()
		// {
		// 	OnTileClick.Invoke(this);
		// }
	}
}