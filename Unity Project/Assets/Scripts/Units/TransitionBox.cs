using ManagersSpace;
using Units;
using UnityEngine;

public class TransitionBox : MonoBehaviour
{
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag(Unit.EnemyTag))
			other.gameObject.layer = UnitsManager.EnemyLayer;
	}
}
