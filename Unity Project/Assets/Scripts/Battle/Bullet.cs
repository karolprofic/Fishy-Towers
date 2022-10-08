using System;
using Behaviours;
using ManagersSpace;
using Units;
using UnityEngine;

namespace Battle
{
	public class Bullet : MovableBehaviour
	{
		//static
		public static readonly string Tag = "Bullet";

		//properties
		[field: SerializeField] public float Damage { get; private set; }
		public Unit Owner { get; private set; }

		//public/inspector
		[SerializeField] public Vector2 direction;

		[SerializeField] private BulletsAttacks.Type bulletAttackType;
		[SerializeField] private Type type;
		[SerializeField] private Movements.Type movementType;
		[SerializeField] private bool multiHit = false;
		[SerializeField] private bool isAlly = false;

		//private
		private Action<Bullet, Collider2D> attack;
		private Action<Bullet, Vector3> movement;
		private bool bocikFlag = false;

		//unity methods
		private void Start()
		{
			attack = BulletsAttacks.GetAction(bulletAttackType);
			movement = Movements.GetAction(movementType);
			if(movementType == Movements.Type.ToDestination && type == Type.BoomerangBullet)
				direction = Owner.transform.position + new Vector3(/*Owner.AreaSize * */3, 0, 0); //multiply by tile size
		}

		//Todo: use callback
		private void Update()
		{
			if(!IsActive || BattleManager.GameStopped)
				return;
			Move();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(isAlly)
			{
				if(other.tag.Equals(Unit.EnemyTag))
					if(multiHit || IsActive)
						attack.Invoke(this, other);
			}
			else
			{
				if(other.tag.Equals(Unit.AllyTag))
					if(multiHit || IsActive)
						attack.Invoke(this, other);
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if(type == Type.BoomerangBullet)
			{
				if(other.gameObject.tag.Equals("LastBoardTile"))
					direction = Owner.transform.position;
				if(other.gameObject.name == Owner.Tile.name)
				{
					if(bocikFlag)
					{
						GoToStorage();
						bocikFlag = false;
					}
					bocikFlag = true;
				}
			}
		}

		//public methods
		public void Shot(Unit owner)
		{
			Owner = owner;
			Row = owner.Row;
			dynamicOffset = transform.position.y * 10 + offset;
			ResetToDefault();
		}

		public override void ResetToDefault()
		{
			bocikFlag = false;
			if(movementType == Movements.Type.ToDestination && type == Type.BoomerangBullet)
				direction = Owner.transform.position + new Vector3(/*Owner.AreaSize * */3, 0, 0);
		}

		//private methods
		private void Move() => movement.Invoke(this, direction);

		//enums
		public enum Type
		{
			SnappingShrimpBullet,
			BoomerangBullet,
			ClownfishBullet,
			SurgeonfishBullet,
			TunaBullet,
			ArcherfishBullet,
			MantisShrimpBullet,
			SeahorseBullet,
			SardinesBullet,
			HerringBullet,
			StarfishBullet,
			SeaWhipBullet,
			KingfishBullet,
			None,
			ToadfishBullet,
			AnglerfishBullet,
			SkatefishBullet,
			ToothfishBullet,
			SwordfishBullet,
			SquidBullet,
			SharkBullet,
			GreenlandSharkBullet
		}
	}
}