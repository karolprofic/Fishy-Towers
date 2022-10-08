using Units;
using UnityEngine;

public class EventCatcher : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Unit unit;
	
	private void Reset()
	{
		animator = GetComponent<Animator>();
		unit = GetComponentInParent<Unit>();
	}

	public void OnDead()
	{
		unit.OnUnitDeadAnimationEnd.Invoke();
	}
}