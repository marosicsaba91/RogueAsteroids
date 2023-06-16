using System;
using UnityEngine;

[Serializable]
public class  GunGroup
{
	public Transform[] guns;
}

public class SpaceshipGun : MonoBehaviour, IGun
{
	[SerializeField] GunGroup[] guns;
	[SerializeField] SpaceshipMovement spaceship;

	[SerializeField] KeyCode shootInput = KeyCode.Space;
	[SerializeField] Projectile projectilePrototype;

	[SerializeField] ImpactGroup team;
	[SerializeField] float initialBulletPerShot = 1;
	[SerializeField] float initialDuration = 10;
	[SerializeField] float initialDamage = 2;
	[SerializeField] float initialSpeed = 10;
	[SerializeField] float initialPush = 1;
	[SerializeField] float initialCooldown = 0.5f;
	[SerializeField] float initialMagnetism = 0.5f;
	[SerializeField, Range(0,1)] float useShipVelocity = 0.7f;
	 
	[SerializeField] FixTicker _fixTicker = new();

	public float BulletPerShot { get; set; }
	public float Cooldown { get; set; }

	public float Damage { get; set; }
	public float Duration { get; set; }
	public float TargetSpeed { get; set; }
	public float Push { get; set; }
	public float Magnetism { get; set; }

	public Transform Transform => transform;
	void Awake()
	{
		_fixTicker.Tick += Shoot;
		AsteroidsGameManager.RestartGame += ResetGame;
	}

	void ResetGame()
	{
		Damage = initialDamage;
		Duration = initialDuration;
		TargetSpeed = initialSpeed;
		Push = initialPush;
		Cooldown = initialCooldown;
		BulletPerShot = initialBulletPerShot;
		Magnetism = initialMagnetism;
		_fixTicker.Reset(); 
		_bulletBudget = 0;
	} 

	float _bulletBudget = 0;

	public ImpactGroup ImpactGroup => team;


	void Shoot(float timeOffset)
	{
		timeOffset += Time.deltaTime;

		_bulletBudget += BulletPerShot;
		int bulletCount = (int)_bulletBudget;
		_bulletBudget -= bulletCount;

		if (bulletCount == 0)
			return;

		int groupIndex = Mathf.Min(guns.Length - 1, bulletCount - 1);
		GunGroup gunGroup = guns[groupIndex];

		for (int i = 0; i < gunGroup.guns.Length; i++)
		{
			Transform gun = gunGroup.guns[i];
			GameObject newProjectileObject = Instantiate(projectilePrototype.gameObject);
			Projectile newProjectile = newProjectileObject.GetComponent<Projectile>();

			Vector2 startSpeed = ((Vector2)gun.up * TargetSpeed) + (spaceship.Velocity * useShipVelocity);
			Vector2 position = (Vector2)gun.position + (timeOffset * startSpeed);
			newProjectile.Setup(this, position, startSpeed); 
		}
	}


	void Update()
	{
		_fixTicker.isRunning = Input.GetKey(shootInput);
		_fixTicker.virtualDeltaTime = Cooldown;
		_fixTicker.Update();
	}
	 
}
