using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
	[SerializeField] SpaceshipInput input;
	[SerializeField] new SpriteRenderer renderer;
	[SerializeField] new Collider2D collider;
	[SerializeField] new Rigidbody2D rigidbody2D;
	[Space]
	[SerializeField, Min(0)] float minAcceleration = 10;
	[SerializeField, Min(0)] float maxAcceleration = 20;
	[SerializeField, Min(0)] float maxSpeed = 10;
	[SerializeField, Min(0)] float angularSpeed = 180;

	public float AccelerationMultiplier { get; internal set; } = 1;
	public float AngularSpeedMultiplier { get; internal set; } = 1;


	void Awake()
	{ 
		AsteroidsGameManager.RestartGame += ResetGame;
	}

	void ResetGame()
	{
		ResetShipPosition();
		AccelerationMultiplier = 1;
		AngularSpeedMultiplier = 1;
	}

	void ResetShipPosition()
	{
		rigidbody2D.velocity = Vector3.zero;
		rigidbody2D.position = Vector3.zero;
		transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

		gameObject.SetActive(true);

	}

	void OnValidate()
	{
		if (renderer == null)
			renderer = GetComponentInChildren<SpriteRenderer>();
		if (collider == null)
			collider = GetComponentInChildren<Collider2D>();
		if (rigidbody2D == null)
			rigidbody2D = GetComponentInChildren<Rigidbody2D>();
		if (input == null)
			input = GetComponentInChildren<SpaceshipInput>();
	}

	public float Drive { get; private set; }
	public bool UseEngine { get; private set; }
	public Vector2 Velocity => rigidbody2D.velocity;

	public float Drag
	{
		get => rigidbody2D.drag;
		set => rigidbody2D.drag = value;
	}
	void FixedUpdate()
	{
		Vector2 inputVec = input.InputVector;
		UseEngine = inputVec.y >= 0;
		Drive = inputVec.y;
		Control(UseEngine, Drive, inputVec.x);
	}

	void Control(bool useDrive, float drive, float turn)
	{
		float acceleration = AccelerationMultiplier * Mathf.Lerp(minAcceleration, maxAcceleration, drive);
		if (useDrive)
			rigidbody2D.velocity = Vector2.MoveTowards(rigidbody2D.velocity, transform.up * maxSpeed, acceleration * Time.deltaTime);

		if (turn != 0)
		{
			float r = rigidbody2D.rotation;
			r += AngularSpeedMultiplier * angularSpeed * Time.deltaTime * -turn;
			rigidbody2D.rotation = r;
		}
	}
}