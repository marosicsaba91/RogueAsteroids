using UnityEngine;

class Projectile : MonoBehaviour, ITeleportHandler
{
	[SerializeField] Rigidbody2D rigidBody;
	[SerializeField] new Renderer renderer;
	[SerializeField] new Collider2D collider;
	[SerializeField] Animator animator;
	[SerializeField] TrailRenderer trail;
	[Space]
	[SerializeField] float destroyDelay = 0.2f;
	[SerializeField] float acceleration = 15;

	[SerializeField] Effect mazzleEffect;
	[SerializeField] Effect dieEffect;
	[SerializeField] Effect hitEffect;

	float _lifeTimeLeft;
	float _targetSpeed;
	bool _destroyed = false;
	float Magnetism => _gun.Magnetism;
	IGun _gun;

	public void Setup(IGun gun, Vector2 position, Vector2 startVelocity)
	{
		_gun = gun;
		float angel = startVelocity.GetAngleToRight();
		transform.rotation = Quaternion.Euler(0, 0, angel - 90);
		_lifeTimeLeft = gun.Duration;
		transform.position = position;
		_targetSpeed = gun.TargetSpeed;
		rigidBody.velocity = startVelocity;
		EffectManager.Play(mazzleEffect, position, angel, gun.Transform);

		AsteroidsGameManager.RestartGame += Die;
		AsteroidManager.AsteroidsCleared += Die;
	}


	void Update()
	{
		_lifeTimeLeft -= Time.deltaTime;

		if (_lifeTimeLeft <= 0)
			animator.SetTrigger("Die");
	}

	Vector2 _magnetism = Vector2.zero;

	void FixedUpdate()
	{
		if (Magnetism != 0)
		{
			Vector2 force = MagnetTarget.ForceVectorAt(transform.position);
			if (force != Vector2.zero)
			{ 
				_magnetism = force * Magnetism;
				rigidBody.velocity += force * (Magnetism * Time.fixedDeltaTime);
			}
		}else
		{
			_magnetism = Vector2.zero;
		}

		float currentSpeed = rigidBody.velocity.magnitude;
		if (currentSpeed != _targetSpeed && currentSpeed != 0)
		{
			float speed = Mathf.MoveTowards(currentSpeed, _targetSpeed, acceleration * Time.fixedDeltaTime);
			rigidBody.velocity *= (speed / currentSpeed);
		}

		transform.rotation = Quaternion.Euler(0, 0, rigidBody.velocity.GetAngleToRight() - 90);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)rigidBody.velocity);
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)_magnetism);
	}

	public void OnTeleport()
	{
		trail.Clear();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent(out ImpactReceiver target))
		{
			Vector2 collisionPoint = other.ClosestPoint(transform.position);
			if (target.TryImpact(rigidBody.velocity, collisionPoint, _gun.Damage, _gun.Push, _gun.ImpactGroup))
			{
				EffectManager.Play(hitEffect, collisionPoint, rigidBody.velocity.GetAngleToRight());
				Destroy();
			}
		}
	}


	public void Die()
	{
		if (_destroyed) return;
		EffectManager.Play(dieEffect, transform.position, rigidBody.velocity.GetAngleToRight());
		Destroy();
	}

	private void Destroy()
	{
		_destroyed = true;
		collider.enabled = false;
		renderer.enabled = false;
		rigidBody.velocity = Vector2.zero;
		Destroy(gameObject, destroyDelay);
	}
}
