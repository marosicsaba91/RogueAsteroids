using System;
using UnityEngine; 

public class ImpactReceiver : MonoBehaviour
{
	[SerializeField] ImpactGroup team;
	[SerializeField] new Collider2D collider2D;
	[SerializeField] Rigidbody2D rigidBody2D;
	[SerializeField] float inertia = 1;
	[SerializeField] float initialMaxHealth;
	[SerializeField] float minimumDamage = 1;
	[SerializeField] bool destroyOnDeath = false;

	[SerializeField] bool enableImpact = true;

	public event Action HealthChanged;
	public event Action LostAllHealth;
	public float Inertia => inertia;
	public float HealthRate => CurrentHealth / MaxHealth;



	void Awake()
	{
		AsteroidsGameManager.RestartGame += Restart;
	}

	void Start()
	{
		Restart();
	}

	void Restart() 
	{
		MaxHealth = initialMaxHealth;
		CurrentHealth = initialMaxHealth;	
	}

	float _maxHealth;
	public float MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = value;
			HealthChanged?.Invoke();
		}
	}
	float _currentHealth;
	public float CurrentHealth
	{
		get => _currentHealth;
		set => _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
	}
	public bool EnableImpact 
	{ 
		get => enableImpact;
		internal set => enableImpact = value;
	} 

	void OnValidate()
	{
		if (collider2D == null)
			collider2D = GetComponent<Collider2D>();
		if (rigidBody2D == null)
			rigidBody2D = GetComponent<Rigidbody2D>();
	}

	public bool TryImpact(Vector2 direction, Vector2 position, float damage, float push, ImpactGroup impactMaker)
	{
		if (!EnableImpact) return false;
		if (!impactMaker.CanInteract(team)) return false;

		damage *= impactMaker.DamageMultiplier(team);
		push *= impactMaker.PushMultiplier(team);
		if (damage < minimumDamage)
			damage = 0;
		Imapct(direction, position, damage, push);

		return true;
	}

	void Imapct(Vector2 direction, Vector2 position, float damage, float push)
	{
		if (CurrentHealth <= 0) return;
		if (rigidBody2D != null)
			rigidBody2D.AddForceAtPosition((push * rigidBody2D.mass) * direction.normalized, position, ForceMode2D.Impulse);

		float lastHealth = CurrentHealth;

		CurrentHealth -= damage;
		CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

		if (lastHealth != CurrentHealth)
		{

			damage = Mathf.Min(damage, lastHealth);
			HealthChanged?.Invoke();
		}

		IImpactHandler[] handlers = GetComponentsInChildren<IImpactHandler>();

		foreach (IImpactHandler handler in handlers)
			handler.OnImpact(this, direction, position, damage, push / inertia);

		if (CurrentHealth == 0)
		{
			LostAllHealth?.Invoke();
			foreach (IImpactHandler handler in handlers)
				handler.OnLostAllHealth(this);

			if (destroyOnDeath)
				Destroy(gameObject);
		}
	}

	public Vector2 ClosestPoint(Vector2 p) => collider2D.ClosestPoint(p);
	internal void Kill() => Imapct(Vector2.zero, rigidBody2D.position, float.MaxValue, 0);
}