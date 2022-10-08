using UnityEngine;

public class EnemyEnterRowTrigger : MonoBehaviour
{
	//public/inspector
	public Battle.Row row;

	//unity methods
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Enemy"))
			row.NewEnemy();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Enemy"))
			row.EnemyLeft();
	}
}