using System;
using System.Collections;
using Units;
using ManagersSpace;

namespace Behaviours
{
	public static class Dies
	{
		public static Action<Unit> GetAction(Type dieType)
		{
			return dieType switch
			{
				Type.Default => Default,
				Type.Explode => Explode,
				_ => Default
			};
		}

		private static void Default(Unit unit)
		{
			unit.ResetToDefault();
			unit.GoToStorage();
			
			//Todo: to nie może tutaj być!!!
			Managers.Statistics.IncreaseStatisticValue("defeatedEnemies", 1);
		}
		
		private static void Explode(Unit unit)
		{
			unit.Animator.SetTrigger(Unit.Death);
			unit.OnUnitDeadAnimationEnd.AddListener(() =>
			{
				var enemies = unit.AreasBox.Enemies;
				foreach (var enemy in enemies)
					enemy.Hurt(unit.Damage);
				Default(unit);
			});
		}
		
		public enum Type
		{
			Default,
			Explode
		}
	}
}