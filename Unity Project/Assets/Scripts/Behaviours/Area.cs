using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.Events;
using Unit = Units.Unit;

namespace Behaviours
{
	public class Area
	{
		//---
		public Tile[] Tiles => tiles;
		
		//---
		
		//properties
		public bool HasEnemies => enemiesAmount > 0;
		public bool HasAllies => alliesAmount > 0;

		//events
		public UnityEvent<Unit> OnAllyEnter = new();
		public UnityEvent<Unit> OnEnemyEnter = new();
		public UnityEvent<Unit> OnAllyExit = new();
		public UnityEvent<Unit> OnEnemyExit = new();

		public UnityEvent<Unit> OnFirstEnemyEnter = new();
		public UnityEvent<Unit> OnLastEnemyExit = new();
		public UnityEvent<Unit> OnFirstAllyEnter = new();
		public UnityEvent<Unit> OnLastAllyExit = new();

		//private
		private int alliesAmount;
		private int enemiesAmount;
		private Tile[] tiles;
		private Areas.Type type;

		private UnityAction<Unit> allyEnterAction;
		private UnityAction<Unit> allyExitAction;
		private UnityAction<Unit> enemyEnterAction;

		private UnityAction<Unit> enemyExitAction;
		
		//constructor
		public Area(Areas.Type type, Vector2Int position, Vector2Int vec, Unit unit)
		{
			if (unit == null || unit.IsAlly)
			{
				allyEnterAction = AllyEnter;
				allyExitAction = AllyExit;
				enemyEnterAction = EnemyEnter;
				enemyExitAction = EnemyExit;	
			}
			else
			{
				allyEnterAction = EnemyEnter;
				allyExitAction = EnemyExit;
				enemyEnterAction = AllyEnter;
				enemyExitAction = AllyExit;
			}
			this.type = type; 

			Update(type, position, vec);
		}

		//public methods
		public void Refresh(Areas.Type type, Vector2Int position, Vector2Int vec)
		{
			Clear();
			Update(type, position, vec);
			enemiesAmount = 0;
			alliesAmount = 0;
			foreach (Tile tile in tiles)
			{
				enemiesAmount += tile.EnemiesAmount;
				alliesAmount += tile.AlliesAmount;
			}
		}

		public List<Unit> getEnemiesInArea()
		{
			List<Unit> enemies = new List<Unit>();
			foreach (var tile in tiles)
			{
				foreach (var enemy in tile.Enemies)
				{
					enemies.Add(enemy);
				}
			}
			return enemies;
		}

		public List<Unit> getAllaysInArea()
		{
			List<Unit> allays = new List<Unit>();
			foreach (var tile in tiles)
			{
				foreach (var ally in tile.Allies)
				{
					allays.Add(ally);
				}
			}
			return allays;
		}


		//private methods
		private void AllyEnter(Unit unit)
		{
			++alliesAmount;
			OnAllyEnter.Invoke(unit);
			if (alliesAmount == 1)
				OnFirstAllyEnter.Invoke(unit);
		}

		private void AllyExit(Unit unit)
		{
			--alliesAmount;
			OnAllyExit.Invoke(unit);
			if (alliesAmount == 0)
				OnLastAllyExit.Invoke(unit);
		}

		private void EnemyEnter(Unit unit)
		{
			++enemiesAmount;
			OnEnemyEnter.Invoke(unit);
			if (enemiesAmount == 1)
				OnFirstEnemyEnter.Invoke(unit);
		}

		private void EnemyExit(Unit unit)
		{
			--enemiesAmount;
			OnEnemyExit.Invoke(unit);
			if (enemiesAmount == 0)
				OnLastEnemyExit.Invoke(unit);
		}

		private void Update(Areas.Type type, Vector2Int position, Vector2Int vec)
		{
			tiles = Areas.GetArea(type, position, vec);

			foreach (Tile tile in tiles)
			{
				tile.OnAllyEnter.AddListener(AllyEnter);
				tile.OnAllyExit.AddListener(AllyExit);
				tile.OnEnemyEnter.AddListener(EnemyEnter);
				tile.OnEnemyExit.AddListener(EnemyExit);
			}
		}

		private void Clear()
		{
			foreach (Tile tile in tiles)
			{
				tile.OnAllyEnter.RemoveListener(allyEnterAction);
				tile.OnAllyExit.RemoveListener(allyExitAction);
				tile.OnEnemyEnter.RemoveListener(enemyEnterAction);
				tile.OnEnemyExit.RemoveListener(enemyExitAction);
			}
		}
	}
}