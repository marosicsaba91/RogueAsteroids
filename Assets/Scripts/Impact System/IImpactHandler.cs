using UnityEngine;

public interface IImpactHandler
{
	void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push);
	void OnLostAllHealth(ImpactReceiver receiver);
}
