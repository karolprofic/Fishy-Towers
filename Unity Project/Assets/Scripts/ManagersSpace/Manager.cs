namespace ManagersSpace
{
	using UnityEngine;

	public abstract class Manager : MonoBehaviour
	{
		protected virtual void Awake()
		{
			Managers.Register(this);
		}
		protected virtual void OnDestroy() => Managers.Unregister(this);
	}
}