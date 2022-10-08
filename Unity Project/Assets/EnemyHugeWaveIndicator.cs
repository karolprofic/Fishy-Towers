using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHugeWaveIndicator : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private AnimationCurve waveCurve;

	private bool indicate = false;
	private float indicateTime = 0;
	private float curveValue = 0;

	public void StartIndicating(float time = 4f)
	{
		indicateTime = time;
		indicate = true;
	}

	private void Update()
	{
		if(!indicate || ManagersSpace.BattleManager.GameStopped) return;
		if(indicateTime <= 0)
		{
			Hide();
			indicate = false;
		}
		indicateTime -= Time.deltaTime;
		curveValue = curveValue >= 1 ? 0 : curveValue + Time.deltaTime;
		canvasGroup.alpha = waveCurve.Evaluate(curveValue);
	}

	private void Hide()
	{
		canvasGroup.alpha = 0;
	}
}