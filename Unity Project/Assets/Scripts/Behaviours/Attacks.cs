using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using ManagersSpace;
using Units;
using UnityEngine;

namespace Behaviours
{
	public static class Attacks
	{
		private static readonly string AttackNumber = "AttackNumber";
		private static Action<Unit, Bullet.Type> _none;
		private static Action<Unit, Bullet.Type> _forwardOneShot;
		private static Action<Unit, Bullet.Type> _forwardDoubleShot;
		private static Action<Unit, Bullet.Type> _forwardDoubleLineShot;
		private static Action<Unit, Bullet.Type> _forwardTripleLineShot;
		private static Action<Unit, Bullet.Type> _forwardSevenShot;
		private static Action<Unit, Bullet.Type> _generateLittleSeaWeed;
		private static Action<Unit, Bullet.Type> _generateSeaWeed;
		private static Action<Unit, Bullet.Type> _generateTwoSeaWeed;
		private static Action<Unit, Bullet.Type> _standingOnUnit;
		private static Action<Unit, Bullet.Type> _differentDirections_5;
		private static Action<Unit, Bullet.Type> _multihit5Enemies;
		private static Action<Unit, Bullet.Type> _onHit;

		static Attacks() => Initialize();

		private static void Initialize()
		{
			_none = None;
			_forwardOneShot = ForwardOneShot;
			_forwardDoubleShot = ForwardDoubleShot;
			_forwardDoubleLineShot = ForwardDoubleLineShot;
			_forwardTripleLineShot = ForwardTripleLineShot;
			_forwardSevenShot = ForwardSevenShot;
			_generateLittleSeaWeed = GenerateLittleSeaWeed;
			_generateSeaWeed = GenerateSeaWeed;
			_generateTwoSeaWeed = GenerateTwoSeaWeed;
			_standingOnUnit = StandingOnUnit;
			_differentDirections_5 = DifferentDirections_5;
			_multihit5Enemies = MultiHit5Enemies;
			_onHit = OnHit;
		}

		public static Action<Unit, Bullet.Type> GetAction(Type attackType)
		{
			return attackType switch
			{
				Type.None => _none,
				Type.ForwardOneShot => _forwardOneShot,
				Type.ForwardDoubleShot => _forwardDoubleShot,
				Type.ForwardDoubleLineShot => _forwardDoubleLineShot,
				Type.ForwardTripleLineShot => _forwardTripleLineShot,
				Type.ForwardSevenShot => _forwardSevenShot,
				Type.GenerateLittleSeaWeed => _generateLittleSeaWeed,
				Type.GenerateSeaWeed => _generateSeaWeed,
				Type.GenerateTwoSeaWeed => _generateTwoSeaWeed,
				Type.StandingOnUnit => _standingOnUnit,
				Type.DifferentDirections_5 => _differentDirections_5,
				Type.Multihit5Enemies => _multihit5Enemies,
				Type.OnHit => _onHit,
				_ => _none
			};
		}

		private static void None(Unit unit, Bullet.Type bulletType)
		{ }

		private static void ForwardOneShot(Unit unit, Bullet.Type bulletType)
		{
			GetBulletAndShot(unit, bulletType);
		}

		private static void ForwardDoubleShot(Unit unit, Bullet.Type bulletType)
		{
			GetBulletAndShot(unit, bulletType);
			unit.StartCoroutine(LateShot(unit, bulletType, 0.4f));
		}

		private static void ForwardDoubleLineShot(Unit unit, Bullet.Type bulletType)
		{
			GetBulletAndShot(unit, bulletType, new Vector3(0, -1, 0));
			GetBulletAndShot(unit, bulletType, new Vector3(0, 1, 0));
		}

		private static void ForwardTripleLineShot(Unit unit, Bullet.Type bulletType)
		{
			GetBulletAndShot(unit, bulletType, new Vector3(0, -1, 0));
			GetBulletAndShot(unit, bulletType, new Vector3(0, 0, 0));
			GetBulletAndShot(unit, bulletType, new Vector3(0, 1, 0));
		}

		private static void ForwardSevenShot(Unit unit, Bullet.Type bulletType)
		{
			GetBulletAndShot(unit, bulletType);
			for (int i = 1; i < 7; i++)
				unit.StartCoroutine(LateShot(unit, bulletType, 0.1f * i, new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0)));
		}

		private static void GenerateLittleSeaWeed(Unit unit, Bullet.Type bulletType)
		{
			Seaweed seaweed = Managers.Seaweed.GetElement(Seaweed.Type.LittleJumping);
			seaweed.ResetToDefault();
			seaweed.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -198);
			seaweed.Generate(unit);
		}

		private static void GenerateSeaWeed(Unit unit, Bullet.Type bulletType)
		{
			Seaweed seaweed = Managers.Seaweed.GetElement(Seaweed.Type.StandardJumping);
			seaweed.ResetToDefault();
			seaweed.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -198);
			seaweed.Generate(unit);
		}
		private static void GenerateTwoSeaWeed(Unit unit, Bullet.Type bulletType)
		{
			Seaweed seaweed = Managers.Seaweed.GetElement(Seaweed.Type.StandardJumping);
			seaweed.ResetToDefault();
			seaweed.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -198);
			seaweed.Generate(unit);
			seaweed = Managers.Seaweed.GetElement(Seaweed.Type.StandardJumping);
			seaweed.ResetToDefault();
			seaweed.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -198);
			seaweed.Generate(unit);
		}
		private static void StandingOnUnit(Unit unit, Bullet.Type bulletType)
		{
			foreach (var enemy in unit.AreasBox.Enemies)
			{
				//Todo: sprawdzić czy warunek możliwy
				if (!enemy.IsActive)
					continue;
				enemy.Hurt(unit.Damage);
			}

		}
		private static void DifferentDirections_5(Unit unit, Bullet.Type bulletType)
		{
			for (int i = 0; i < 5; i++)
			{
				Bullet bullet = Managers.Bullets.GetElement(bulletType);
				Vector3 unitPosition = unit.transform.position;
				bullet.transform.position = unitPosition;

				bullet.direction = new Vector2(Mathf.Sin(72 * i * Mathf.Deg2Rad), Mathf.Cos(72 * i * Mathf.Deg2Rad)); //some internet guy math (but it's great)

				bullet.Shot(unit);
			}

		}
		private static void MultiHit5Enemies(Unit unit, Bullet.Type bulletType)
		{
			if (unit.IsAlly)
			{
				unit.Animator.SetInteger(AttackNumber,UnityEngine.Random.Range(0,6));
				Attack5Units(unit, unit.AreasBox.Enemies, Color.white);
			}
			else
			{
				var enemies = unit.AreasBox.Enemies;
				enemies.Reverse();
				Attack5Units(unit, enemies, Color.black);
			}

		}

		private static void OnHit(Unit unit, Bullet.Type bulletType)
		{
			//it's probably not good place for this...
		}

		private static IEnumerator LateShot(Unit unit, Bullet.Type bulletType, float time, Vector3? offPosition = null)
		{
			yield return new WaitForSeconds(time);
			if (!unit.IsActive)
				yield break;
			GetBulletAndShot(unit, bulletType, offPosition);
		}

		private static void GetBulletAndShot(Unit unit, Bullet.Type bulletType, Vector3? offPosition = null)
		{
			Bullet bullet = Managers.Bullets.GetElement(bulletType);
			Vector3 unitPosition = unit.transform.position;
			bullet.transform.position = unitPosition + offPosition ?? unitPosition;
			bullet.Shot(unit);
		}

		private static void Attack5Units(Unit unit, List<Unit> units, Color color)
		{
			Vector3[] paths = new Vector3[6];
			paths[0] = unit.transform.position;
			int unitCounter = 0;
			foreach (var otherUnit in units)
			{
				if (!otherUnit.IsActive)
					continue;
				paths[unitCounter + 1] = otherUnit.transform.position;
				otherUnit.Hurt(unit.Damage);
				if (++unitCounter > 4)
				{
					break;
				}
			}
			for (int i = 0; i < unitCounter; i++)
			{
				DrawLine(paths[i], paths[i + 1], color);
			}
		}

		//source: https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
		private static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
		{
			//Todo: Zrobić jakiś magazyn linii lub dodać je do BulletManager jeżeli to możliwe
			//Todo: żeby uniknąć ciągłego instancjonowania obiektów

			GameObject myLine = new GameObject();
			myLine.transform.position = start;
			myLine.AddComponent<LineRenderer>();
			LineRenderer lr = myLine.GetComponent<LineRenderer>();
			//lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiplied")); //TODO: add shader
			lr.material.SetColor("_Color", color);
			lr.startWidth = 0.1f;
			lr.endWidth = 0.1f;
			lr.generateLightingData = true;
			lr.SetPosition(0, start);
			lr.SetPosition(1, end);
			GameObject.Destroy(myLine, duration);
		}

		public enum Type
		{
			None,
			ForwardOneShot,
			ForwardDoubleShot,
			ForwardDoubleLineShot,
			ForwardTripleLineShot,
			ForwardSevenShot,
			GenerateLittleSeaWeed,
			GenerateSeaWeed,
			GenerateTwoSeaWeed,
			StandingOnUnit,
			DifferentDirections_5,
			Multihit5Enemies,
			OnHit
		}
	}
}