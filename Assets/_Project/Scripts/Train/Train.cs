using UnityEngine;

public class Train : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float acceleration = 10f;
	[SerializeField] float deceleration = 10f;
	[SerializeField] float maxSpeed = 15f;

	[Header("Technical")]
	[SerializeField] float movementSpeed;
	[SerializeField] float wagonLength;
	[SerializeField] GameObject[] wagonsObjects;

	int _wagonsCount;
	float _currentSpeed;

	public float Speed => _currentSpeed;


	private void Start()
	{
		_wagonsCount = wagonsObjects.Length;
	}

	public void Accelerate(short direction)
	{
		_currentSpeed += acceleration * Time.deltaTime * direction;
		_currentSpeed = Mathf.Clamp(_currentSpeed, -maxSpeed, maxSpeed);
	}

	public void Decelerate()
	{

		if (_currentSpeed > 0.01f)
			_currentSpeed -= deceleration * Time.deltaTime;
		else if (_currentSpeed < -0.01f)
			_currentSpeed += deceleration * Time.deltaTime;
		else
			_currentSpeed = 0f;
	}

	public void Scroll(float speed)
	{
		float delta = speed * Time.deltaTime;

		// Scroll train
		for (int i = 0; i < _wagonsCount; i++)
		{
			wagonsObjects[i].transform.position += new Vector3(delta, 0f);
		}
	}
}
