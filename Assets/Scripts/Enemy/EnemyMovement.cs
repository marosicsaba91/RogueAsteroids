using UnityEngine;

class EnemyMovement : MonoBehaviour
{
	[SerializeField] float targetSpeed = 2; 
	[SerializeField] float acceleration = 1;
	[SerializeField] float minDistanceToTarget = 1;
	[SerializeField] float angularSpeed = 360;
	[SerializeField] float minDelayToChangeTarget = 2;
	[SerializeField] float maxDelayToChangeTarget = 4;
	[SerializeField] float margin = 0.05f;

	[SerializeField] Rigidbody2D rb;
	[SerializeField]  EnemyGun gun;

	Vector2 _target;

	bool _search = false;

	void OnValidate()
	{
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();
		if (gun == null)
			gun = GetComponent<EnemyGun>();
	}

	void Start()
	{
		SetTarget();
	}

	public void SetTarget()
	{
		float x = Random.Range(margin, 1 - margin);
		float y = Random.Range(margin, 1 - margin);
		_target = Camera.main.ViewportToWorldPoint(new Vector3(x,y,0));
		_search = false;
	}

	void FixedUpdate()
	{ 
		Vector2 selfPosition = transform.position; 
		Vector2 targetDirection = (_target - selfPosition);
		float distance = targetDirection.magnitude - minDistanceToTarget;
		Vector2 targetVelocity = Vector2.zero;

		if (distance > 0)
			targetVelocity = targetDirection.normalized * targetSpeed;

		targetVelocity += (Vector2) EnemyAvoidObjects.GetPushVector(selfPosition, gameObject); 

		rb.velocity = Vector2.MoveTowards(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

		if (!_search)
		{
			float targetDistance = Vector2.Distance(_target, selfPosition);
			if (targetDistance < minDistanceToTarget)
			{
				_search = true;
				gun.FireAll();
				float delayToChangeTarget = Random.Range(minDelayToChangeTarget, maxDelayToChangeTarget);
				Invoke(nameof(SetTarget), delayToChangeTarget);
			}
		}

		rb.angularVelocity = angularSpeed;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(_target, minDistanceToTarget);
	}
}