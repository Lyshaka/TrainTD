using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
	[SerializeField] GameObject enemiesParent;


	private void Start()
	{
		
	}

	public void Scroll(float speed)
	{
		float delta = speed * Time.deltaTime;

		enemiesParent.transform.position += new Vector3(delta, 0f);
	}
}
