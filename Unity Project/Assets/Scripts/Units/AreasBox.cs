using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Events;

public class AreasBox : MonoBehaviour
{
	//properties
	public List<AreaCollider> Colliders => colliders;

	public int AlliesInRange => _allies.Count;
	public int EnemiesInRange => _enemies.Count;

	public bool HasAlliesInRange => _allies.Count > 0;
	public bool HasEnemiesInRange => _enemies.Count > 0;

	public List<Unit> Allies => new(_allies);
	public List<Unit> Enemies => new(_enemies);

	//public/inspector
	[SerializeField] private List<AreaCollider> colliders = new();
	[SerializeField] private Unit unit;

	//events
	public readonly UnityEvent<Unit> OnAllyEnter = new();
	public readonly UnityEvent<Unit> OnAllyExit = new();
	public readonly UnityEvent<Unit> OnEnemyEnter = new();
	public readonly UnityEvent<Unit> OnEnemyExit = new();

	//private
	private readonly List<Unit> _allies = new();
	private readonly List<Unit> _enemies = new();
	private string allyTag;
	private string enemyTag;

	//unity methods
	private void Start()
	{
		(allyTag, enemyTag) = unit.CompareTag(Unit.AllyTag)
			? (Unit.AllyTag, Unit.EnemyTag)
			: (Unit.EnemyTag, Unit.AllyTag);
	}

	//public methods
	public void OnEnter(Collider2D col)
	{
		if(col.CompareTag(allyTag))
		{
			var ally = col.GetComponent<Unit>();
			_allies.Add(ally);
			OnAllyEnter.Invoke(ally);
			return;
		}

		if(col.CompareTag(enemyTag))
		{
			var enemy = col.GetComponent<Unit>();
			_enemies.Add(enemy);
			unit.Animator.SetBool("EnemiesInRange", true);
			OnEnemyEnter.Invoke(enemy);
		}
	}

	public void OnExit(Collider2D col)
	{
		if(col.CompareTag(allyTag))
		{
			var ally = col.GetComponent<Unit>();
			_allies.Remove(ally);
			OnAllyExit.Invoke(ally);
			return;
		}

		if(col.CompareTag(enemyTag))
		{
			var enemy = col.GetComponent<Unit>();
			_enemies.Remove(enemy);
			if(_enemies.Count == 0)
				unit.Animator.SetBool(Unit.Enemies, false);
			OnEnemyExit.Invoke(enemy);
		}
	}
}