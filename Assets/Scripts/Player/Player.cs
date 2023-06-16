using System;
using UnityEngine;

class Player : SingletonMonoBehaviour<Player>
{
	[SerializeField] SpaceshipMovement spaceship;
	[SerializeField] ImpactReceiver spaceshipImpactReceiver;
	[SerializeField] SpaceshipGun gun;
	[SerializeField] SpaceshipMovement movement;
	[SerializeField] KeyCode debugDestroy = KeyCode.Alpha0;

	public static event Action HealthChanged;
	public static event Action SpaceshipDied;

	public static SpaceshipGun Gun => Instance.gun;
	public static SpaceshipMovement Movement => Instance.movement;
	public static GameObject Spaceship => Instance.gameObject;

	private void OnValidate()
	{
		if (spaceship == null)
			spaceship = GetComponentInChildren<SpaceshipMovement>();
		if (spaceshipImpactReceiver == null)
			spaceshipImpactReceiver = GetComponentInChildren<ImpactReceiver>();
		if (gun == null)
			gun = GetComponentInChildren<SpaceshipGun>(); 
		if (movement == null)
			movement = GetComponentInChildren<SpaceshipMovement>();
	}

	public static void SpaceshipDie() => SpaceshipDied?.Invoke();
	public static float MaxHealth
	{
		get => Instance.spaceshipImpactReceiver.MaxHealth;
		set => Instance.spaceshipImpactReceiver.MaxHealth = value;
	}


	public static float CurrentHealth
	{
		get => Instance.spaceshipImpactReceiver.CurrentHealth;
		set => Instance.spaceshipImpactReceiver.CurrentHealth = value;

	}

	protected sealed override void SingletonAwake()
	{
		spaceshipImpactReceiver.HealthChanged += () => HealthChanged.Invoke();
		spaceshipImpactReceiver.LostAllHealth += () => SpaceshipDie();
	}

	void Update()
	{
		if(Application.isEditor && Input.GetKeyDown(debugDestroy))
			spaceshipImpactReceiver.Kill();
	}
}

