using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

struct AsteroidSpawnData
{
	public Asteroid prefab;
	public Vector2 position;
	public Vector2 velocity;
	public float angularVelocity;
	public bool spawnEffect;
}

class AsteroidManager : SingletonMonoBehaviour<AsteroidManager>
{
	[SerializeField] Asteroid[] startAsteroids;
	[SerializeField] float minDistance = 5;
	[SerializeField] float maxDistance = 8;

	[SerializeField] float maxStartAngularVelocity = 5;
	[SerializeField] float maxStartVelocity = 40;

	[SerializeField] float maxChangeAngularVelocity = 2;
	[SerializeField] float maxChangVelocity = 20;


	[SerializeField] KeyCode debugKillAsteroidsButton = KeyCode.K;
	[SerializeField] AnimationCurve sumAsteroidWightOverLevel;
	[SerializeField] float spawnAsteroidDelay;


	public static readonly List<Asteroid> asteroidsInGame = new();

	static readonly List<Asteroid> _spawnableAssets = new();
	static readonly List<Collider2D> _spawnedAsteroids = new();
	public static event Action AsteroidsCleared;


	protected sealed override void SingletonAwake()
	{
		AsteroidsGameManager.NewLevelLoaded += SpawnAsteroidsToLevel;
		AsteroidsGameManager.RestartGame += KillAllAsteroidsQuick;
	}

	public static void RegisterAsteroid(Asteroid asteroid)
	{
		asteroidsInGame.Add(asteroid);
	}

	void Update()
	{
		if (Application.isEditor && Input.GetKeyDown(debugKillAsteroidsButton))
			KillAllAsteroids();
	}


	void KillAllAsteroids()
	{
		foreach (Asteroid asteroid in FindObjectsOfType<Asteroid>())
			asteroid.Kill();
	}

	void KillAllAsteroidsQuick()
	{
		foreach (Asteroid asteroid in FindObjectsOfType<Asteroid>())
			Destroy(asteroid.gameObject);
	}


	public static void UnregisterAsteroid(Asteroid asteroid)
	{
		if (ApplicationHelper.IsQuitting) return;
		if (!asteroidsInGame.Remove(asteroid)) return;

		if (asteroidsInGame.Count == 0)
			AsteroidsCleared?.Invoke();
	}

	void SpawnAsteroidsToLevel(int level)
	{
		float fullWeight = Instance.sumAsteroidWightOverLevel.Evaluate(level - 1);
		SpawnAsteroidsToWeight(fullWeight);
	}

	public static void SpawnSubAsteroids(Vector2 position, Vector2 velocity, float angularVelocity, List<Asteroid> subAsteroids)
	{
		Instance.StartCoroutine(SpawnAsteroids(Spawn()));

		IEnumerable<AsteroidSpawnData> Spawn()
		{
			for (int i = 0; i < subAsteroids.Count; i++)
			{
				AsteroidSpawnData newAsteroid = new()
				{
					prefab = subAsteroids[i],
					position = position
				};

				float angle = Random.Range(0, 2 * MathF.PI);
				Vector2 velocityChange = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(-Instance.maxChangVelocity, Instance.maxChangVelocity);
				newAsteroid.velocity = velocity + velocityChange;
				float angularVelocityChange = Random.Range(-Instance.maxChangeAngularVelocity, Instance.maxChangeAngularVelocity);
				newAsteroid.angularVelocity = angularVelocity + angularVelocityChange;

				yield return newAsteroid;
			}
		}
	}

	void SpawnAsteroidsToWeight(float fullWeight)
	{
		StartCoroutine(SpawnAsteroids(Spawn(), spawnAsteroidDelay));

		IEnumerable<AsteroidSpawnData> Spawn()
		{
			_spawnableAssets.Clear();
			_spawnableAssets.AddRange(Instance.startAsteroids);
			// _spawnableAssets.Sort((a, b) => b.AsteroidWeight.CompareTo(a.AsteroidWeight));

			while (_spawnableAssets.Count > 0)
			{
				Asteroid prefab = _spawnableAssets[Random.Range(0, _spawnableAssets.Count)];
				if (prefab.AsteroidWeight > fullWeight)
				{
					_spawnableAssets.Remove(prefab);
					continue;
				}
				AsteroidSpawnData newAsteroid = new()
				{
					prefab = prefab,
				};

				float angle = Random.Range(0f, 2 * Mathf.PI);
				float distance = Random.Range(Instance.minDistance, Instance.maxDistance);
				newAsteroid.position = distance * new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

				angle = Random.Range(0, 2 * MathF.PI);
				newAsteroid.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(-maxStartVelocity, maxStartVelocity);
				newAsteroid.angularVelocity = Random.Range(-maxStartAngularVelocity, maxStartAngularVelocity);
				newAsteroid.spawnEffect = true;
				fullWeight -= prefab.AsteroidWeight;
				yield return newAsteroid;

				if (fullWeight == 0)
					break;
			}
		}
	}

	static int _asteroidNumber = 0;

	static IEnumerator SpawnAsteroids(IEnumerable<AsteroidSpawnData> prefabs, float delay = 0)
	{
		_spawnedAsteroids.Clear();

		foreach (AsteroidSpawnData data in prefabs)
		{
			if (delay > 0)
				yield return new WaitForSeconds(delay);

			Asteroid newAsteroid = Instantiate(data.prefab);
			newAsteroid.name = data.prefab.name + "  -  " + _asteroidNumber.ToString();
			_spawnedAsteroids.Add(newAsteroid.Collider);
			_asteroidNumber++;

			if (data.spawnEffect && newAsteroid.spawnEffect != null)
			{
				EffectManager.Play(newAsteroid.spawnEffect, data.position, 0);
			}

			newAsteroid.transform.position = data.position;
			newAsteroid.Velocity = data.velocity;
			newAsteroid.AngularVelocity = data.angularVelocity;

			newAsteroid.gameObject.SetActive(true);
		}

		yield return null;
		CollisionManager.OnEnableColliders(_spawnedAsteroids);
	}
}
