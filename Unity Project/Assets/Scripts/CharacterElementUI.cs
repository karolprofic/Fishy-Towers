using UnityEngine;
using UnityEngine.UI;

public class CharacterElementUI : MonoBehaviour
{
	[SerializeField] private Image elementImage;

	public void SetImage(Sprite image)
	{
		elementImage.sprite = image;
	}
}