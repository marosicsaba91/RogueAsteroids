using UnityEngine;

public class ImpactEffectPlayer : MonoBehaviour, IImpactHandler
{
	[SerializeField] Effect[] impactEffects;
	[SerializeField] Effect[] deathEffects;
	[SerializeField] float minimumDamage = 0.5f;
	[SerializeField] float minimumPush = 0;
	[SerializeField] bool disableOnDeath = true;

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push)
	{
		if (minimumDamage > damage) return;
		if (minimumPush > push) return;
		if (impactEffects.Length == 0) return;

		Effect effect = impactEffects[Random.Range(0, impactEffects.Length)];
		EffectManager.Play(effect, position, direction.GetAngleToRight());
	}

	public void OnLostAllHealth(ImpactReceiver receiver)
	{ 
		if (disableOnDeath)
			gameObject.SetActive(false);

		if (deathEffects.Length == 0) return;

		Effect effect = deathEffects[Random.Range(0, deathEffects.Length)];
		if (effect != null)
			EffectManager.Play(effect, transform.position, 0);
	}
}
