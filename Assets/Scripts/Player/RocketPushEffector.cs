using UnityEngine;

public class RocketPushEffector : MonoBehaviour
{ 
    [SerializeField] SpaceshipMovement spaceship;
	[SerializeField] AnimationCurve scaleOverDrive;
	[SerializeField] AnimationCurve forceOverDrive;
	[SerializeField] AnimationCurve forceOverDistance;
	[SerializeField] new Collider2D collider;

	[SerializeField, Range(-1,1)] float driveTest;
	[SerializeField] float maxForce = 10;
	[SerializeField] ImpactGroup team;

	readonly Collider2D[] _damageTargets = new Collider2D[10];
	static ContactFilter2D _contactFilter = new();
	float _scale;

	void OnValidate()
	{
		if (spaceship == null)
			spaceship = GetComponentInParent<SpaceshipMovement>();
		if (collider == null)
			collider = GetComponent<Collider2D>();

		UpdateScale(driveTest);

	}

	void Update()
	{
		float drive = spaceship.Drive;
		UpdateScale(drive);
	}	
	
	void UpdateScale(float drive)
	{
		_scale = scaleOverDrive.Evaluate(drive);
		transform.localScale = new Vector3(_scale, _scale, 1);
	}
	 
	void FixedUpdate()
	{
		int n = Physics2D.OverlapCollider(collider, _contactFilter, _damageTargets);

		for (int i = 0; i < n; i++)
		{
			if (!_damageTargets[i].TryGetComponent(out ImpactReceiver damageTarget)) 
				continue;

			Vector2 direction = (Vector2)transform.up;
			float distance = transform.InverseTransformPoint(damageTarget.transform.position).magnitude;
			float force = 
				forceOverDistance.Evaluate(distance) * 
				forceOverDrive.Evaluate(spaceship.Drive) *
				maxForce * Time.fixedDeltaTime;

			Vector2 closestPoint = damageTarget.ClosestPoint(transform.position);
			damageTarget.TryImpact(direction.normalized, closestPoint, 0, force, team);			
		}
		
	}

}
