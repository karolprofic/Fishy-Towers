using System;
using System.Collections.Generic;
using Battle;
using Units;
using UnityEngine;
using UnityEngine.Events;

namespace ManagersSpace
{
	public class UnitsManager : MagazineManager<Unit.Type, Unit>
	{
		//static events
		public static readonly UnityEvent<Unit> OnAnyUnitDied = new();
		public static readonly UnityEvent<Unit> OnUnitPlaced = new();

		public static int AllyLayer { get; private set; }
		public static int EnemyLayer { get; private set; }
		public static int TransitionLayer { get; private set; }

		//properties
		public Unit selectedUnit { get; private set; }

		//public/inspector
		[SerializeField] private Sprite errorImage;
		[SerializeField] private MiniaturesPackage[] miniaturesPackages;

		//private
		private readonly Dictionary<Unit.Type, Sprite> miniatures = new();
		private Unit.Type lastSelected;

		//unity methods
		protected override void Awake()
		{
			base.Awake();
			foreach(var (unitType, miniature) in miniaturesPackages)
				miniatures.TryAdd(unitType, miniature);
			
			AllyLayer = LayerExt.GetLayerIdx("Ally");
			EnemyLayer = LayerExt.GetLayerIdx("Enemy");
			TransitionLayer = LayerExt.GetLayerIdx("Transition");
		}

		protected override void Start()
		{
			base.Start();
			UnitsChoosingBoardElement.OnPlayerSelectorClick.AddListener(OnPlayerSelectorClick);
			Tile.OnTileClick.AddListener(OnTileClick);
		}

		protected void Update()
		{
			if(BattleManager.GameStopped) return;
			foreach(var (_, magazine) in magazines)
			{
				magazine.ForEach(unit =>
				{
					if(!unit.IsActive)
						return;
					unit.UpdateUnit();
				});
			}

			if(!selectedUnit) return;

			//Todo: camera main
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = -1;
			selectedUnit.transform.position = pos;
		}

		//public methods
		public Sprite GetMiniature(Unit.Type unitType)
		{
			if(miniatures.TryGetValue(unitType, out Sprite miniature))
				return miniature;
			Debug.LogError($"Couldn't find the Image for {unitType}");
			return errorImage;
		}

		public Unit GetUnit(Unit.Type type)
		{
			return GetElement(type);
		}

		public int UnitCost(Unit.Type type)
		{
			foreach(var (t, unit) in elementsPackages)
			{
				if(t == type)
					return unit.Cost;
			}
			Debug.LogError("Unit not found");
			return 0;
		}

		public void PlaceUnitOnTile(Tile tile, Unit unit)
		{
			if(!tile.TrySetUnit(unit))
				return;

			unit.PutOnTile(tile);
			OnUnitPlaced.Invoke(unit);
		}

		//private methods
		private void OnPlayerSelectorClick(Unit.Type selector)
		{
			if(selector == Unit.Type.None
				|| selector == lastSelected)
			{
				lastSelected = Unit.Type.None;
				if(selectedUnit == null)
					return;
				selectedUnit.GoToStorage();
				selectedUnit = null;
				return;
			}

			foreach(var (type, element) in elementsPackages)
			{
				if(type == selector)
				{
					if(!Managers.Seaweed.CanBuy(element.Cost))
					{
						return;
					}
					break;
				}
			}

			lastSelected = selector;
			if(selectedUnit != null)
				selectedUnit.GoToStorage();
			selectedUnit = GetElement(selector);
		}

		private void OnTileClick(Tile tile)
		{
			if(!tile.TrySetUnit(selectedUnit))
				return;

			selectedUnit.PutOnTile(tile);
			Managers.Seaweed.Buy(selectedUnit.Cost);
			selectedUnit = null;
			lastSelected = Unit.Type.None;
		}

		//structs
		[Serializable]
		private struct MiniaturesPackage
		{
			public Unit.Type Type;
			public Sprite Miniature;

			public void Deconstruct(out Unit.Type unitType, out Sprite miniature)
			{
				unitType = Type;
				miniature = Miniature;
			}
		}
	}
}