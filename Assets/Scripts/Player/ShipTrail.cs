using System;
using UnityEngine;

[Serializable]
struct TrailState
{
	public Vector3 position;
	public Vector3 scale;
	public float alpha;
	public float emission;
	public float startSpeed;
	public float startScale;

	public static TrailState Lerp(TrailState one, TrailState other, float t) => new()
	{
		position = Vector3.Lerp(one.position, other.position, t),
		scale = Vector3.Lerp(one.scale, other.scale, t),
		alpha = Mathf.Lerp(one.alpha, other.alpha, t),
		emission = Mathf.Lerp(one.emission, other.emission, t),
		startScale= Mathf.Lerp(one.startScale, other.startScale, t),
		startSpeed = Mathf.Lerp(one.startSpeed, other.startSpeed, t),
	};
}

class ShipTrail : MonoBehaviour
{
	[SerializeField] new ParticleSystem particleSystem;
	[SerializeField] SpaceshipMovement ship;

	[Space]
	[SerializeField] TrailState noEngineUseState = new();
	[SerializeField] TrailState minDriveState = new ();
	[SerializeField] TrailState maxDriveState = new();
	
	[Space]
	[SerializeField] float growSpeed = 1;
	[SerializeField] float shrinkSpeed = 1;
	[Space]
	[SerializeField, Range(-1,1)] float testSpeed;

	float _size = -1;

	void OnValidate()
	{
		if (ship == null)
			ship = GetComponentInParent<SpaceshipMovement>();

		if (particleSystem == null)
			particleSystem = GetComponentInChildren<ParticleSystem>();
		 

		SetTrailTransform(testSpeed);
	}

	void Update()
	{
		if (ship == null || particleSystem == null)
			return;

		float targetSize = ship.UseEngine ? ship.Drive : -1;
		float resizeSpeed = targetSize > _size ? growSpeed : shrinkSpeed;
		_size = Mathf.MoveTowards(_size, targetSize, resizeSpeed * Time.deltaTime);

		SetTrailTransform(_size);
	}

	void SetTrailTransform(float speed)
	{
		float t = speed < 0 ? 1 + speed : speed;

		TrailState state = speed < 0 ?
			TrailState.Lerp(noEngineUseState, minDriveState, t) :
			TrailState.Lerp(minDriveState, maxDriveState, t);

		// Transform
		transform.localPosition = state.position;
		transform.localScale = state.scale;

		// Alpha
		float alpha = state.alpha;
		ParticleSystem.ColorOverLifetimeModule colorModule = particleSystem.colorOverLifetime;
		Gradient gradient = colorModule.color.gradient;
		gradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0), new GradientAlphaKey(alpha, 1) };
		colorModule.color = gradient;

		// Emission
		ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
		emissionModule.rateOverTime = state.emission;

		// Speed
		ParticleSystem.MainModule mainModule = particleSystem.main;
		mainModule.startSize = state.startScale;
		mainModule.startSpeed = state.startSpeed;

		
	}
}
