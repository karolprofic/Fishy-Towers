using Battle;
using ManagersSpace;
using UnityEngine;

public abstract class MovableBehaviour : MonoBehaviour, IMovable
{
	//serialized properties
	[field: SerializeField] public float MovementSpeed { get; protected set; }
	[field: SerializeField] public bool IsActive { get; protected set; }
	
	//properties
	public Row Row { get; set; }
	public float Offset => dynamicOffset;
	
	//public/inspector
	[SerializeField] protected float offset;
	//Todo: dodać aktualizację 
	[SerializeField] protected float dynamicOffset;
	
	//unity methods
	protected virtual void LateUpdate()
	{
		UpdateYOffset();
	}
	
	//public methods
	public virtual void GetFromStorage()
	{
		IsActive = true;
	}

	public virtual void GoToStorage()
	{
		IsActive = false;
		transform.position = Managers.Game.storageMapLocation;
	}

	public abstract void ResetToDefault();
	
	//private methods
	private void UpdateYOffset()
	{
		Vector3 position = transform.position;
		position.z = 0.3f * ((position.x + 7.0f) * 0.5f * dynamicOffset);
		transform.position = position;
	}
}