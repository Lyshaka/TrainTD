using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float moveSpeed = 2f;
	[SerializeField] float turnSpeed = 30f;
	[SerializeField] float distanceToReach = 2f;
	[SerializeField] Transform target;

	Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector2 dir = target.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		//Debug.Log("Angle : " + angle);

		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			Quaternion.Euler(0, 0, angle),
			turnSpeed * Time.deltaTime
		);

		//Debug.Log("Distance : " + Vector2.Distance(rb.position, target.position));

		//if (Vector2.Distance(transform.position, target.position) > distanceToReach)
		//	rb.linearVelocity = transform.right * moveSpeed;
		//else
		//	rb.linearVelocity = Vector2.zero;

			rb.linearVelocity = transform.right * moveSpeed;
	}
}
