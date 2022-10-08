using System;
using ManagersSpace;
using Units;
using UnityEngine;

namespace Behaviours
{
	public static class SpecialPowers
	{
		public static Action<Unit, float> GetAction(Type typeType)
		{
			return typeType switch
			{
				Type.None => None,
				Type.Poisoned => Poisoned,
				_ => None
			};
		}

		private static void None(Unit obj, float value)
		{
			// TODO: implements
		}

		private static void Poisoned(Unit unit, float value)
		{
			if(unit.Health <=0)
				return;
			unit.Hurt(value * Time.deltaTime);
		}
		
		//enums
		public enum Type
		{
			None,
            IgnitePassingBullets,
            BoostNeighboursAttackSpeed,
            Poisoned
		}
	}
}
