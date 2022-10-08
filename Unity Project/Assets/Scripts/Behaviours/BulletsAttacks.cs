using System;
using Battle;
using Units;
using UnityEngine;

namespace Behaviours
{
	public static class BulletsAttacks
	{
		private static Action<Bullet, Collider2D> _none;
		private static Action<Bullet, Collider2D> _oneHit;
		private static Action<Bullet, Collider2D> _freezing;
		private static Action<Bullet, Collider2D> _fireball;
		private static Action<Bullet, Collider2D> _piercing;
		private static Action<Bullet, Collider2D> _boomerang;
		private static Action<Bullet, Collider2D> _stunning;
		private static Action<Bullet, Collider2D> _luring;


		static BulletsAttacks() => Initialize();

		private static void Initialize()
		{
			_none = None;
			_oneHit = OneHit;
			_freezing = Freezing;
			_fireball = Fireball;
			_piercing = Piercing;
			_boomerang = Boomerang;
			_stunning = Stunning;
			_luring = Luring;

		}

		public static Action<Bullet, Collider2D> GetAction(Type bulletType)
		{
			return bulletType switch
			{
				Type.OneHit => _oneHit,
				Type.Freezing => _freezing,
				Type.Fireball => _fireball,
				Type.Piercing => _piercing,
				Type.Boomerang => _boomerang,
				Type.Stunning => _stunning,
				Type.Luring => _luring,
				_ => _none
			};
		}

		private static void None(Bullet bullet, Collider2D collider)
		{ }

		private static void OneHit(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();
			unitToHurt.Hurt(bullet.Damage);
			//Todo: Invoke animation before GoToStorage. Implement it in a Bullet class.
			bullet.GoToStorage();
		}

		private static void Freezing(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();
			unitToHurt.Hurt(bullet.Damage);
			unitToHurt.SlowDown(0.5f, 3);
			//Todo: Invoke animation before GoToStorage. Implement it in a Bullet class.
			bullet.GoToStorage();
		}

		private static void Fireball(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();
			unitToHurt.Hurt(bullet.Damage * 2);
			//Todo: Invoke animation before GoToStorage. Implement it in a Bullet class.
			bullet.GoToStorage();
		}

		private static void Piercing(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();

			unitToHurt.Hurt(bullet.Damage);
		}

		private static void Boomerang(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();

			unitToHurt.Hurt(bullet.Damage);
		}

		private static void Stunning(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();
			unitToHurt.Hurt(bullet.Damage);
			unitToHurt.Stun(2);
			//Todo: Invoke animation before GoToStorage. Implement it in a Bullet class.
			bullet.GoToStorage();
		}

		private static void Luring(Bullet bullet, Collider2D collider)
		{
			if (bullet.Owner.CompareTag(collider.tag))
				return;
			if (collider.CompareTag("Barrier"))
				return;

			var unitToHurt = collider.GetComponent<Unit>();
			unitToHurt.Hurt(bullet.Damage);
			unitToHurt.MoveToRow(bullet.Owner.Row);
			//Todo: Invoke animation before GoToStorage. Implement it in a Bullet class.
			bullet.GoToStorage();
		}

		//enums
		public enum Type
		{
			OneHit,
			Freezing,
			Fireball,
			Piercing,
			Boomerang,
			Stunning, //surgeon
			Luring, //clown
			None
		}

	}
}