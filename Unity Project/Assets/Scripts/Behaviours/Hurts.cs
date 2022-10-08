using System;
using Units;

namespace Behaviours
{
	public static class Hurts
	{
		public static Action<Unit, float> GetAction(Type type)
		{
			return type switch
			{
				Type.Default => Default,
				Type.Armour => Armour,
				Type.Shield => Shield,
				_ => Default
			};
		}

		private static void Default(Unit unit, float damage)
		{
			if(unit.Health <= 0)
				return;
			unit.ForceHurt(damage);
		}

		private static void Armour(Unit unit, float damage)
		{
			// TODO: implement
		}

		private static void Shield(Unit unit, float damage)
		{
			// TODO: implement
		}
		
		//enums
		public enum Type
		{
			Default,	
			Armour,		//decrease damage
			Shield		//block damage
		}
	}
}