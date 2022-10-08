using System.Collections.Generic;
using System.Linq;
using Behaviours;
using Units;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
	public class Board : MonoBehaviour
	{
		//properties
		public List<Row> Rows => rows;
		public Row this[int idx]  {
			get
			{
				try
				{
					return rows[idx];
				}
				catch
				{
					Debug.Log(idx);
				}

				return null;
			}
		}
		
		//public/inspector
		[SerializeField]
		//Todo: zamienić na dwuwymiarową tablicę
		private List<Row> rows;
		//private Tile[][] tiles;

		//public events
		public UnityEvent<Tile> OnAllyChanged = new();
		
		//unity methods
		private void Awake()
			=> Areas.Initialize(this);

		private void Start()
		{
			for (int i = 0; i < rows.Count; ++i)
			for (int j = 0; j < rows[0].Count; ++j)
			{
				int ci = i;
				int cj = j;
				rows[i][j].OnAllayChanged.AddListener(
					() => OnAllyChanged.Invoke(rows[ci][cj]));
			}
		}

		private void Reset()
		{
			rows = GetComponentsInChildren<Row>().ToList();
			/*for (int i = 0; i < rows.Count; ++i)
			{
				Transform t = rows[i].transform;
				t.position = transform.position + (t.localScale.y) * Vector3.up + Garbage.Offset.y * Vector3.up;
				rows[i].Set();
			}*/
		}

		//public method
		public bool TryAddAlly(Unit ally, Vector2Int pos)
			=> TryGetTile(pos, out Tile tile) && tile.TrySetUnit(ally);

		public bool TryGetTile(Vector2Int pos, out Tile tile)
		{
			if (Contains(pos))
			{
				try
				{
					tile = rows[pos.y][pos.x];
					return true;
				}
				catch
				{
					tile = null;
					return false;
				}
				
			}

			tile = null;
			return false;
		}
		
		public bool Contains(Vector2Int pos)
			=> pos.x >= 0 && pos.x <= rows.Count - 1 && pos.y >= 0 && pos.y <= rows[0].Count - 1;
	}
}