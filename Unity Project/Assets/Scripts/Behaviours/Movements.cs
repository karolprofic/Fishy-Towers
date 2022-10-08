using System;
using UnityEngine;

namespace Behaviours
{
	public static class Movements
	{
		private static float tileWidth = 2.2f;
		public static Action<MovableBehaviour, Vector3> GetAction(Type movementType)
		{
			return movementType switch
			{
				Type.Static => Static,
				Type.Forward => Forward,
				Type.Backward => Backward,
				Type.Directed => Directed,
				Type.ToDestination => ToDestination,
				_ => Static
			};
		}

		public static void Static(MovableBehaviour unit, Vector3 direction) { }

		public static void Forward(MovableBehaviour movable, Vector3 ignored)
		{
			Directed(movable, Vector3.right);
		}

		public static void Backward(MovableBehaviour movable, Vector3 ignored)
		{
			Directed(movable, Vector3.left);
		}

		public static void Directed(MovableBehaviour movable, Vector3 direction)
		{
			Transform transform = movable.transform;
			Vector3 position = transform.position + Time.deltaTime * movable.MovementSpeed * direction * tileWidth;
			transform.position = position;
		}

		public static void ToDestination(MovableBehaviour unit, Vector3 destination)
		{
			Transform transform = unit.transform;
			Vector3 position = transform.position;
			transform.position = Vector3.MoveTowards(position, destination, Time.deltaTime * unit.MovementSpeed);
		}

		//enums
		public enum Type
		{
			Static,
			Forward,
			Backward,
			Directed,
			ToDestination
		}
	}
}