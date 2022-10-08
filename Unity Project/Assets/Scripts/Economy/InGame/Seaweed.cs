using System;
using UnityEngine;
using Behaviours;
using Units;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using ManagersSpace;

public class Seaweed : MovableBehaviour
{
	//static
	public static Seaweed selectedSeaweed;
	
	//Todo: properties z wielkiej litery
	//public/inspector
	[field: SerializeField] public ushort value { get; private set; }

	[SerializeField] private Type type;

	[field: SerializeField] public Vector3 destination { get; set; }
	[SerializeField] private Movements.Type movementType;
	[field: SerializeField] public float lifespan { get; private set; }
	[field: SerializeField] public bool isJumpy { get; private set; }
	[SerializeField] public Collider2D collider2D;
	
	public Unit Generator { get; private set; }

	public static readonly UnityEvent<Seaweed> OnSeaweedClick = new();

	//private
	private Action<Seaweed, Vector3> movement;
	private bool jumpedAlready = false;

	//unity methods
	private void Start()
	{
		movement = Movements.GetAction(movementType);
	}

	protected override void LateUpdate() {
		
	}

	private void Update()
	{
		if(!IsActive || BattleManager.GameStopped)
			return;
		
		if(Input.GetMouseButtonDown(0))
		{
			if(selectedSeaweed != null)
				return;

			var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (!collider2D.OverlapPoint(position))
				return;
                
			selectedSeaweed = this;
		}
		
		if(lifespan < 0)
		{
			GoToStorage();
		}

		if(isJumpy)
		{
			if(!jumpedAlready)
			{
				destination = Generator.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), -198);
				destination = new Vector3(destination.x, destination.y, -198);
				jumpedAlready = true;
			}
			if(transform.position == destination && destination.y > Generator.transform.position.y)
			{
				destination = new Vector3(destination.x, Generator.transform.position.y, -198);
			}
		}

		lifespan -= Time.deltaTime;

		Move();
	}

	//public methods
	public override void ResetToDefault()
	{
		lifespan = 10;
		jumpedAlready = false;
	}

	public void Generate(Unit generator)
	{
		Generator = generator;
	}

	//private methods
	private void Move() => movement.Invoke(this, destination);

	// private void OnMouseUpAsButton()
	// {
	// 	OnSeaweedClick.Invoke(this);
	// }

	//enums
	public enum Type
	{
		Standard,
		Little,
		StandardJumping,
		LittleJumping
	}
}