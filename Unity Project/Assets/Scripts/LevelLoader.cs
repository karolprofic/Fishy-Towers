using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	//public/inspector
	public new string name = "";
	public CanvasGroup imageGroup;
	public Animator animator;

	//unity methods
	private void Awake()
	{
		imageGroup.alpha = 1.0f;
	}

	//private methods
	private IEnumerator LoadScene(string name_)
	{
		//Todo: wyciągnąć na początek klasy private static readonly int Start = Animator.StringToHash("Start");
		Debug.Log("Start load");
		animator.SetTrigger("Start");
		yield return new WaitForSeconds(1f);
		Debug.Log("load");
		SceneManager.LoadScene(name_);
	}
	
	//public methods
	public void LoadLevel(string name_)
	{
		StartCoroutine(LoadScene(name_));
	}
}