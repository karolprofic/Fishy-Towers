using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Behaviours
{
	public static class Areas
	{
		private static Board board;

		private static readonly Vector2Int[] startPositions =
		{
			new(0, 0), 	new(0,-1), 	new(0,-2), 	new(0,-3), 	new(0,-4), 
			new(1,0), 	new(2,0), 	new(2,-1), 	new(3,-1), 	new(4,-1), 	new(5,-1), 	new(5,-2), 	new(6,-2), 
			new(1,1), 	new(1,2), 	new(2,2), 	new(2,3), 	new(3,3), 
			new(-1,0), 	new(-2,0), 	new(-2,-1), new(-3,-1),	new(-4,-1), new(-5,-1), new(-5,-2), 	new(-6,-2), 
			new(-1,1), 	new(-1,2), 	new(-2,2), 	new(-2,3), 	new(-3,3)
		};
		
		private static Func<Vector2Int, Vector2Int, Tile[]> _none;
		private static Func<Vector2Int, Vector2Int, Tile[]> _horizontalLinePointToLength;
		private static Func<Vector2Int, Vector2Int, Tile[]> _horizontalLinePointToEnd;
		private static Func<Vector2Int, Vector2Int, Tile[]> _horizontalLineStartToEnd;
		private static Func<Vector2Int, Vector2Int, Tile[]> _verticalLineStartToEnd;
		private static Func<Vector2Int, Vector2Int, Tile[]> _crossLineStartToEnd;
		private static Func<Vector2Int, Vector2Int, Tile[]> _rect;
		private static Func<Vector2Int, Vector2Int, Tile[]> _circle;
		private static Func<Vector2Int, Vector2Int, Tile[]> _fullBoardForBattleManager;
		private static Func<Vector2Int, Vector2Int, Tile[]> _star;
		public static void Initialize(Board _board)
		{
			board = _board;
		}

		public static Tile[] GetArea(Type areaType, Vector2Int position, Vector2Int vec)
		{
			return areaType switch
			{
				Type.None => None(position, vec),
				Type.HorizontalLinePointToLength => HorizontalLinePointToLength(position, vec),
				Type.HorizontalLinePointToEnd => HorizontalLinePointToEnd(position, vec),
				Type.HorizontalLineStartToEnd => HorizontalLineStartToEnd(position, vec),
				Type.VerticalLineStartToEnd => VerticalLineStartToEnd(position, vec),
				Type.CrossLineStartToEnd => CrossLineStartToEnd(position, vec),
				Type.Rect => Rect(position, vec),
				Type.Circle => Circle(position, vec),
				Type.FullBoardForBattleManager => FullBoardForBattleManager(position, vec),
				Type.Star => Star(position, vec),
			};
		}

		private static Tile[] None(Vector2Int position, Vector2Int vec)
			=> Array.Empty<Tile>();

		private static Tile[] HorizontalLinePointToLength(Vector2Int position, Vector2Int vec)
		{
			if (!board.TryGetTile(position, out Tile tile))
				return Array.Empty<Tile>();

			Row row = tile.row;
			int elements =  (Mathf.Abs(vec.x) + 1).Clamp(0, row.Count);
			Tile[] tiles = new Tile[elements];

			int idx = (vec.x < 0 ? position.x + vec.x : position.x).Clamp(0, row.Count - 1);
			
			for (int i = 0; i < elements; ++i)
				tiles[i] = row[idx + i];

			return tiles;
		}

		private static Tile[] HorizontalLinePointToEnd(Vector2Int position, Vector2Int vec)
		{
			if (!board.TryGetTile(position, out Tile tile))
				return Array.Empty<Tile>();

			Row row = tile.row;
			int elements = row.Count - tile.index - 2; // - 2 na - 1
			Tile[] tiles = new Tile[elements];

			for (int i = 0; i < elements; ++i)
				tiles[i] = row[tile.index + i];

			return tiles;
		}

		private static Tile[] HorizontalLineStartToEnd(Vector2Int position, Vector2Int vec)
		{
			if (!board.TryGetTile(position, out Tile tile))
				return Array.Empty<Tile>();

			Row row = tile.row;
			Tile[] tiles = new Tile[row.Count - 2]; // - 2 na - 1

			for (int i = 0; i < tiles.Length; ++i) // - 2 na - 1
				tiles[i] = row[i];

			return tiles;
		}

		private static Tile[] VerticalLineStartToEnd(Vector2Int position, Vector2Int vec)
		{
			if (!board.TryGetTile(position, out Tile tile))
				return Array.Empty<Tile>();

			int index = tile.index;

			Tile[] tiles = new Tile[board.Rows.Count];

			for (int i = 0; i < tiles.Length; ++i)
				tiles[i] = board[i][index];

			return tiles;
		}

		private static Tile[] CrossLineStartToEnd(Vector2Int position, Vector2Int vec)
		{
			if (!board.TryGetTile(position, out Tile tile))
				return Array.Empty<Tile>();

			int index = tile.index;
			Row row = tile.row;
			int len1 = board.Rows.Count;
			int len2 = row.Count - 2;
			Tile[] tiles = new Tile[len1 + len2 - 1];

			int idx = 0;
			for (; idx < len1; ++idx)
				tiles[idx] = board[idx][index];

			for (int i = 0; i < len2; ++i)
			{
				if (i == index)
					continue;
				tiles[idx++] = row[i];
			}
			return tiles;
		}

		private static Tile[] Rect(Vector2Int position, Vector2Int vec)
		{
			if (!board.TryGetTile(position, out Tile tile))
				return Array.Empty<Tile>();

			Vector2Int corner1 = new Vector2Int(
				(position.x - vec.x).Clamp(0, tile.row.Count - 2),
				(position.y - vec.x).Clamp(0, board.Rows.Count - 1));

			Vector2Int corner2 = new Vector2Int(
					(position.x + vec.x).Clamp(0, tile.row.Count - 2),
					(position.y + vec.x).Clamp(0, board.Rows.Count - 1));

			Tile[] tiles = new Tile[(corner2.x - corner1.x + 1) * (corner2.y - corner1.y + 1)];

			int idx = 0;
			for (int j = corner1.x; j <= corner2.x; ++j)
				for (int i = corner1.y; i <= corner2.y; ++i)
					tiles[idx++] = board[i][j];

			return tiles;
		}

		private static Tile[] Circle(Vector2Int position, Vector2Int vec)
		{
			// TODO: implement
			throw new NotImplementedException();
		}

		private static Tile[] FullBoardForBattleManager(Vector2Int position, Vector2Int vec)
		{
			Tile[] tiles = new Tile[board.Rows.Count * board.Rows[0].Count];

			int idx = 0;
			for (int i = 0; i < board.Rows.Count; ++i)
				for (int j = 0; j < board.Rows[0].Count; ++j)
					tiles[idx++] = board[i][j];

			return tiles;
		}

		private static Tile[] Star(Vector2Int position, Vector2Int vec)
		{
			List<Tile> tiles = new();
			
			for (int i = 0; i < startPositions.Length; ++i)
				if(board.TryGetTile(startPositions[i] + position, out Tile tile))
					tiles.Add(tile);
			
			return tiles.ToArray();
		}
		
		public enum Type
		{
			None,
			HorizontalLinePointToLength,
			HorizontalLinePointToEnd,
			HorizontalLineStartToEnd,
			VerticalLineStartToEnd,
			CrossLineStartToEnd,
			Rect,
			Circle,
			FullBoardForBattleManager,
			Star
		}
	}
}