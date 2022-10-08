using System.Collections.Generic;
using Battle;
using Units;
using Behaviours;
using UnityEngine;
using UnityEngine.Events;

namespace ManagersSpace
{
	public class BattleManager : Manager
	{
		public static bool GameStopped = false;

		//public/inspector
		[field: SerializeField] public Board Board { get; private set; }
		public Vector3 Offset = Vector3.zero;
		public List<Unit.Type> startUnits = new();
		public List<Unit.Type> startSeaweedGeneratorUnits = new();
		public int howMuchToDraw = 4;
		public ushort hugeWavesToWin { get; set; }

		[SerializeField] private WinScreen winScreen;
		[SerializeField] private UnitsChoosingBoard unitsChoosingBoard;
		[SerializeField] private EnemyGenerator enemyGenerator;
		[SerializeField] private float timeBetweenWaves = 5f;
		[SerializeField] private float timeBeforeStartSpawning = 1f;

		//events
		public readonly UnityEvent OnWon = new();
		public readonly UnityEvent OnLose = new();
		public readonly UnityEvent OnPause = new();
		public readonly UnityEvent OnUnPause = new();

		//private
		private float timeToNextWave;
		private bool firstWaveSend = false;
		private bool waveInPreparation = false;
		private Area area;

		//unity methods
		protected override void Awake()
		{
			base.Awake();
			StopGame();
		}

		private void Start()
		{
			timeToNextWave = timeBeforeStartSpawning;
			firstWaveSend = false;
			waveInPreparation = false;
			OnWon.AddListener(() =>
			{
				Managers.Progression.OnWon();
				winScreen.Open();
			});
			OnLose.AddListener(() =>
			{
				Managers.Progression.OnLose();
				StopGame();
			});
			OnPause.AddListener(StopGame);
			OnUnPause.AddListener(ResumeGame);
		}

		private void Update()
		{
			if(GameStopped) return;
			if(Input.GetMouseButtonUp(0))
			{
				if(Tile.selectedTile == null)
					return;

				if(Tile.selectedTile.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
					Tile.OnTileClick.Invoke(Tile.selectedTile);
				Tile.selectedTile = null;
			}
			if(waveInPreparation) return;
			if(enemyGenerator.HugeWaveCounter >= hugeWavesToWin)
			{
				if(!area.HasEnemies)
				{
					StopGame();
					OnWon.Invoke();
				}
				return;
			}
			if(timeToNextWave < 0 || (!area.HasEnemies && firstWaveSend)) //add Area on full board
			{
				waveInPreparation = true;
				enemyGenerator.SendWave();
				timeToNextWave = timeBetweenWaves;
				firstWaveSend = true;
			}

			timeToNextWave -= Time.deltaTime;
		}

		//public methods

		public void StartGame()
		{
			hugeWavesToWin = Managers.Progression.getHugeWavesNeededToWin();
			GameStopped = false;
			area = new Area(Areas.Type.FullBoardForBattleManager, new Vector2Int(0, 0), new Vector2Int(0, 0), null);

			area.OnEnemyEnter.AddListener(Unit =>
				{
					if(!Unit.IsAlly)
						waveInPreparation = false;
				});
		}

		public void StopGame()
		{
			GameStopped = true;
		}

		public void ResumeGame()
		{
			GameStopped = false;
		}
	}
}