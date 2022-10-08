using UnityEngine;

public class AreaCollider : MonoBehaviour
{
	//public/inspector
	[SerializeField] private AreasBox areasBox;
	
	//unity methods
	private void Reset()
	{
		areasBox = GetComponentInParent<AreasBox>();
		areasBox.Colliders.Add(this);
	}

	//private methods
	private void OnTriggerEnter2D(Collider2D col) => areasBox.OnEnter(col);
	private void OnTriggerExit2D(Collider2D col) => areasBox.OnExit(col);
}