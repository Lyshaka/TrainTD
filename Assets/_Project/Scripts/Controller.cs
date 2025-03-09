using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float cameraSpeed = 10f;
	[SerializeField] float cameraSmoothTime = 0.1f;

	[Header("References")]
	[SerializeField] GroundGeneration groundGeneration;
	[SerializeField] Train train;
	[SerializeField] EnemiesManager enemies;

	[Header("Debug")]
	[SerializeField] TextMeshProUGUI debugText;

	// Camera
	private float _targetCameraSpeed = 0f;
	private float _cameraVelocity = 0f;
	private float _smoothCameraSpeed;
	private float _deltaCameraTrain = 0f;

	// Stats
	private float _traveledDistance;


	private void Update()
	{
		// Scroll train
		if (Input.GetKey(KeyCode.D))
			train.Accelerate(-1);
		else if (Input.GetKey(KeyCode.A))
			train.Accelerate(1);
		else
			train.Decelerate();

		// Scroll camera
		if (Input.GetKey(KeyCode.RightArrow))
			_targetCameraSpeed = -cameraSpeed;
		else if (Input.GetKey(KeyCode.LeftArrow))
			_targetCameraSpeed = cameraSpeed;
		else
			_targetCameraSpeed = 0f;

		_smoothCameraSpeed = Mathf.SmoothDamp(_smoothCameraSpeed, _targetCameraSpeed, ref _cameraVelocity, cameraSmoothTime);


		_traveledDistance -= train.Speed * Time.deltaTime;

		_deltaCameraTrain += _smoothCameraSpeed * Time.deltaTime;

		debugText.text = $"Train Speed : {Mathf.Abs(train.Speed):#0.00}\n" +
			$"Distance traveled : {_traveledDistance:#0.00}\n" +
			$"Camera/Train delta : {-_deltaCameraTrain:#0.00}";

		// Apply scrolls
		train.Scroll(_smoothCameraSpeed);
		groundGeneration.Scroll(train.Speed + _smoothCameraSpeed);
		enemies.Scroll(train.Speed + _smoothCameraSpeed);
	}
}
