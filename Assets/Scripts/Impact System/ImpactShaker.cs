using System;
using System.Collections.Generic;
using UnityEngine;

class SineWave
{
	public Vector2 direction;
	public float amplitude;
	public float frequency;
	public float start;

	public Vector2 Evaluate(float time)
	{
		return amplitude * Mathf.Sin(frequency * time - start) * direction;
	}
}
[Serializable]
class SineSetup
{
	[Range(0, 1)] public float directional;
	[Range(0, 1)] public float amplitude = 1;
	public float frequency = 10;
}

public class ImpactShaker : MonoBehaviour, IImpactHandler
{
	[SerializeField] Transform shaked;
	[SerializeField] SineSetup[] sines;
	[SerializeField] float pushMultiplier = 0.1f;
	[SerializeField, Min(0)] float maxWeight = 100;
	[SerializeField, Range(0, 1)] float weightEffect = 0.5f;
	[SerializeField] float drag;

	// Impact
	[SerializeField] float minimumDamage = 0.5f;
	[SerializeField] float minimumPush = 0.5f;

	readonly List<SineWave> sineWaves = new ();

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push)
	{
		if (minimumDamage > damage) return;
		if (minimumPush > push) return;
		 
		sineWaves.Clear();
		Vector2 dir = direction.normalized;
		foreach (SineSetup setup in sines)
		{
			Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
			Vector2 sideDir = Vector2.Lerp(randomDirection, dir, setup.directional);
			float weightRate = Mathf.Clamp(receiver.Inertia, 0 , maxWeight) / maxWeight;
			float pushWeight = Mathf.Lerp(1, weightRate, weightEffect);
			SineWave wave = new()
			{
				direction = sideDir, 
				amplitude = setup.amplitude * push * pushWeight * pushMultiplier,
				frequency = setup.frequency,
				start = Time.time
			};
			sineWaves.Add(wave);
		}
	}

	void Update()
	{
		if (sineWaves.Count == 0) return;
		Vector2 total = Vector2.zero;
		for (int i = sineWaves.Count - 1; i >= 0; i--)
		{
			SineWave wave = sineWaves[i];
			if (wave.amplitude <= 0.001f)
				sineWaves.Remove(wave);
			else
			{
				total += wave.Evaluate(Time.time);
				wave.amplitude *= 1f - (drag * Time.deltaTime);
			}
		}
		shaked.position = shaked.parent.position + (Vector3)total;
	}

	public void OnLostAllHealth(ImpactReceiver receiver) { }
}
