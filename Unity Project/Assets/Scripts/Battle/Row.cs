using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Battle
{
	public class Row : MonoBehaviour
	{
		public bool EnemyInRow => enemyCount > 0;

		[SerializeField]
		private List<Tile> tiles = new();

		public int Count => tiles.Count;
		public Tile this[int i] => tiles[i];
		public int index;
		
		
		private int enemyCount = 0;

		public void NewEnemy()
		{
			enemyCount++;
		}

		public void EnemyLeft()
		{
			enemyCount--;
		}

		//unity methods
		private void Reset()
		{
			tiles = GetComponentsInChildren<Tile>().ToList();
			Set();
		}

		public void Set()
		{
			for (int i = 0; i < tiles.Count; ++i)
			{
				Transform t = tiles[i].transform;
				if (i == 0)
					continue;
				t.position = tiles[i - 1].transform.position + t.localScale.x * Vector3.right + Garbage.TileOffset;
			}
		}
	}
}