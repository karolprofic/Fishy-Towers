using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using Units;
using ManagersSpace;
using Random = UnityEngine.Random;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
	//public/inspector

	[SerializeField] private EnemyHugeWaveIndicator enemyHugeWaveIndicator;
	[SerializeField] private Tile[] startPositions;
	[SerializeField] private EnemiesValuePackage[] enemiesValuePackage;

	[SerializeField] private ushort wavePower;
	[SerializeField] private ushort waveStrengthAdder;
	[SerializeField] private ushort unitPerWaveCap;
	[SerializeField] private float timeBetweenEnemiesInWave;
	[SerializeField] private ushort wavesPerHugeWave;
	[SerializeField] private float hugeWaveMultiplayer;

	public ushort WaveCounter = 1;
	public ushort HugeWaveCounter = 0;

	//public methods
	public void SendWave()
	{
		ushort holder = wavePower;
		if(WaveCounter % wavesPerHugeWave == 0)
		{
			wavePower = (ushort)(wavePower * hugeWaveMultiplayer);
			HugeWaveCounter++;
			enemyHugeWaveIndicator.StartIndicating();
		}
		DeployEnemies(GetUnits(ChooseFighters()));
		wavePower = holder;
		wavePower += waveStrengthAdder; //maybe it should be multiplied at some point
		WaveCounter++;
	}

	//private methods
	private Dictionary<Unit.Type, ushort> ChooseFighters() //abomination I know :)
	{
		Dictionary<Unit.Type, ushort> unitsOfTypeChosen = new();
		for(int i = 0; i < enemiesValuePackage.Length; i++)
		{
			unitsOfTypeChosen.Add(enemiesValuePackage[i].Type, 0);
		}

		unitsOfTypeChosen[enemiesValuePackage[0].Type] = wavePower;

		int unitCap = unitPerWaveCap; //maybe divide by less

		for(int i = 0; i < enemiesValuePackage.Length; i++)
		{
			while(!(unitCap > unitsOfTypeChosen.ElementAt(i).Value))
			{
				if(i + 1 >= enemiesValuePackage.Length)
					break;
				unitsOfTypeChosen[enemiesValuePackage[i].Type] -= enemiesValuePackage[i + 1].NumberOfUnitsOfPrevHardness;
				unitsOfTypeChosen[enemiesValuePackage[i + 1].Type]++;
			}
		}
		return unitsOfTypeChosen;
	}

	private List<Unit> GetUnits(Dictionary<Unit.Type, ushort> unitsOfTypeToGet)
	{
		List<Unit> listOfUnits = new();
		foreach(var dic in unitsOfTypeToGet)
		{
			//Debug.Log(dic.ToString());
			for(int i = 0; i < dic.Value; i++)
			{
				listOfUnits.Add(Managers.Units.GetUnit(dic.Key));
			}
		}
		return listOfUnits;
	}

	private void DeployEnemies(List<Unit> enemies)
	{
		ushort[] temp = { 0, 0, 0, 0, 0 };
		List<ushort> unitsPlacedOnTile = new List<ushort>(temp);
		foreach(var enemy in enemies)
		{
			ushort numberOfTileToPlaceOn = (ushort)Random.Range(0, 5);
			if((int)numberOfTileToPlaceOn == unitsPlacedOnTile.IndexOf(unitsPlacedOnTile.Max()))
				numberOfTileToPlaceOn = (ushort)Random.Range(0, 5); //just to lower probability
			enemy.StartCoroutine(LatePlaceEnemy(enemy, numberOfTileToPlaceOn, timeBetweenEnemiesInWave * unitsPlacedOnTile[numberOfTileToPlaceOn] + (Random.value * 2)));
			unitsPlacedOnTile[numberOfTileToPlaceOn]++;
		}
	}

	private IEnumerator LatePlaceEnemy(Unit enemy, ushort numberOfTileToPlaceOn, float time)
	{
		yield return new WaitForSeconds(time);
		if(enemy.Placed)
			yield break;
		Managers.Units.PlaceUnitOnTile(startPositions[numberOfTileToPlaceOn], enemy);
		enemy.transform.position = enemy.transform.position + new Vector3(0, Random.Range(-0.3f, 0.3f), 0);
	}

	//struct
	[Serializable]
	private struct EnemiesValuePackage
	{
		public Unit.Type Type;
		public ushort NumberOfUnitsOfPrevHardness;

		public void Deconstruct(out Unit.Type unitType, out ushort numberOfUnitsOfPrevHardness)
		{
			unitType = Type;
			numberOfUnitsOfPrevHardness = NumberOfUnitsOfPrevHardness;
		}
	}
}