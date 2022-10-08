using UnityEngine;
using TMPro;
using ManagersSpace;

public class SeaweedCounter : MonoBehaviour
{
	public TMP_Text seaweedText;

	private void Update()
	{
		seaweedText.text = $"{Managers.Seaweed.currentAmount}";
	}
}