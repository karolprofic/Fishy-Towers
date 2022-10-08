using System;

[Serializable]
public struct ElementPackage<T, TY> where T : Enum where TY : MovableBehaviour
{
	public T Type;
	public TY Element;

	public void Deconstruct(out T type, out TY bullet)
	{
		type = Type;
		bullet = Element;
	}
}