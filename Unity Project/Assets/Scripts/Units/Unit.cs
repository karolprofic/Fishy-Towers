using System;
using Battle;
using Behaviours;
using ManagersSpace;
using UnityEngine;
using UnityEngine.Events;

namespace Units
{
	public class Unit : MovableBehaviour
	{
		//static
		public static readonly string AllyTag = "Ally";
		public static readonly string EnemyTag = "Enemy";

		private static readonly int Placed1 = Animator.StringToHash("Placed");
		private static readonly int Attack1 = Animator.StringToHash("Attack");
		public static readonly int Death = Animator.StringToHash("Death");
		public static readonly int Enemies = Animator.StringToHash("EnemiesInRange");
		
		//properties
		public bool Placed
		{
			get => _placed;
			private set
			{
				_placed = value;
				animator.SetBool(Placed1, value);
			}
		}

		//public Area Area { get; private set; }
		public AreasBox AreasBox => areasBox;
		public Tile Tile { get; set; }

		public Animator Animator => animator;


		//serialized properties
		[field: SerializeField] public float Health { get; private set; }
		//???
		[field: SerializeField] public float Damage { get; private set; }
		[field: SerializeField] public float FireRate { get; private set; }
		[field: SerializeField] public ushort Cost { get; private set; }
		[field: SerializeField] public float RefreshTIme { get; private set; }
		[field: SerializeField] public bool IsStatic { get; private set; }
		//[field: SerializeField] public float AreaSize { get; private set; }

		//public/inspector
		public bool IsAlly;

		[SerializeField] private Vector2 direction;

		[SerializeField] private Animator animator;
		[SerializeField] private Attacks.Type attackType;
		[SerializeField] private Hurts.Type hurtType; //we don't have enemy with armour yet, but ok
		[SerializeField] private SpecialPowers.Type specialPowerTypeType;  // boosting ect. - working all the time
		[SerializeField] private Dies.Type dieType;
		[SerializeField] private Movements.Type movementType;
		[SerializeField] private Areas.Type areaType;

		[SerializeField] private Bullet.Type bulletType;

		[SerializeField] private Type type;

		[SerializeField] private Collider2D collider;

		[SerializeField] private AreasBox areasBox;
		[SerializeField] private float specialPowerValue;
		
		[SerializeField] private AudioClip attackSound;
		
		//events
		public readonly UnityEvent OnDied = new();
		public readonly UnityEvent OnUnitDeadAnimationEnd = new();

		//private
		private Action<Unit, Bullet.Type> attack;
		private Action<Unit, float> hurt;
		private Action<Unit, float> specialPower;
		private Action<Unit> die;
		private Action<Unit, Vector3> movement;

		private float lastShotTime;
		private BackUp backUp;
		
		private bool hasArea;
		private bool _placed;
		private int originalMask;
		

		//unity methods
		private void Awake()
		{
			backUp.Health = Health;
			collider.enabled = false;
			OnDied.AddListener(() => UnitsManager.OnAnyUnitDied.Invoke(this));
			LoadBehaviours();
			originalMask = gameObject.layer;
		}

		private void Start()
		{
			hasArea = areasBox != null;
		}

		public void UpdateUnit()
		{
			if(!Placed) return;
			
			lastShotTime += Time.deltaTime;
			SpecialPower();
			
			if (!areasBox.HasEnemiesInRange && areaType != Areas.Type.None)
			{
				Move();
				return;
			}

			if (lastShotTime < FireRate)
				return;

			lastShotTime = UnityEngine.Random.Range(-0.3f,0.3f);
			Attack();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			//Todo: Sprawdzić czy nie ma tego w Tile
			if (other.CompareTag("LastBoardTile") || other.CompareTag(Tile.Tag))
			{
				Tile = other.GetComponent<Tile>();
			}
		}

		//public methods
		public void Attack()
		{
			if(attackSound != null)
				Managers.Audio.SoundsSource.PlayOneShot(attackSound);
			attack.Invoke(this, bulletType);
			animator.SetTrigger(Attack1);
		}

		public void Hurt(float damage) => hurt.Invoke(this, damage);

		public void SpecialPower() => specialPower.Invoke(this, specialPowerValue);

		public void ForceHurt(float damage)
		{
			Health -= damage;
			if (Health > 0.0f)
				return;
			Health = 0.0f;
			Die();
		}

		public void PutOnTile(Tile tile)
		{
			var tran = transform;
			tran.position = tile.placePosition.position;
			Placed = true;
			collider.enabled = true;
			dynamicOffset = tran.position.y * 10 + offset;
		}

		public override void GetFromStorage()
		{
			base.GetFromStorage();
			if(IsAlly) return;
			gameObject.layer = UnitsManager.TransitionLayer;
		}

		public override void GoToStorage()
		{
			base.GoToStorage();
			_placed = false;
			OnUnitDeadAnimationEnd.RemoveAllListeners();
			collider.enabled = false;
		}

		public void SlowDown(float power, float time)
		{
			//TODO: implement
		}

		public void Stun(float time)
		{
			//TODO: implement
		}

		public void MoveToRow(Row targetedRow)
		{
			//TODO: implement
		}

		public override void ResetToDefault()
		{
			Health = backUp.Health;
		}

		//private methods
		private void Die()
		{
			if (Tile != null)
				Tile.UnitOnTile = false;
			die.Invoke(this);
			OnDied.Invoke();
		}

		private void Move() => movement.Invoke(this, Vector3.zero);

		private void LoadBehaviours()
		{
			attack = Attacks.GetAction(attackType);
			hurt = Hurts.GetAction(hurtType);
			specialPower = SpecialPowers.GetAction(specialPowerTypeType);
			die = Dies.GetAction(dieType);
			movement = Movements.GetAction(movementType);
		}

		//enums
		public enum Type
		{
			None,
			Coral,
			LittleCoral,
			TableCoral,
			Turtle,
			TubeSponges,
			DoubleCoral,
			SnappingShrimp,
			Jellyfish,
			Pufferfish,
			LavaChannel,
			Anemone,
			Boomerang,
			Clownfish,
			Surgeonfish,
			Tuna,
			Archerfish,
			MantisShrimp,
			Seahorse,
			Fluke,
			DoverSole,
			Sardines,
			Herring,
			Starfish,
			SeaWhip,
			Kingfish,
			Toadfish,
			Anglerfish,
			Skatefish,
			Eel,
			Toothfish,
			Swordfish,
			Squid,
			Shark,
			GreenlandShark
		}

		public enum Site
		{
			Ally,
			Enemy
		}

		//structs
		private struct BackUp
		{
			public float Health;
		}
	}
}
