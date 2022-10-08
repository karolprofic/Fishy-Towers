public interface IMovable
{
	public float MovementSpeed { get; }
	public bool IsActive { get; }

	public void GoToStorage();

	public void ResetToDefault();
	
	public enum Type
	{
	}
}