using System.Collections.Generic;
using UnityEngine;

class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
	[SerializeField] List<Vector2> spawnPoints;
	[SerializeField] GameObject enemyPrefabsSmall;
	[SerializeField] GameObject enemyPrefabsBig;
	[SerializeField] AnimationCurve nextEnemySpawnDurationOverTime1;
	[SerializeField] AnimationCurve nextEnemySpawnDurationOverTime2;
	[SerializeField] AnimationCurve probabilityOfBigEnemyOverTime;
	[SerializeField] AnimationCurve healthMultiplierOverTime;

	[SerializeField] KeyCode spawnSmallEnemyDebugCode = KeyCode.Alpha1;
	[SerializeField] KeyCode spawnBigEnemyDebugCode = KeyCode.Alpha2;

	bool _paused = false;
	float _time = 0;
	float _nextEnemySpawnTime;


	protected override void SingletonAwake()
	{
		AsteroidManager.AsteroidsCleared += () => _paused = true;
		AsteroidsGameManager.NewLevelLoaded += (_) => _paused = false;
		AsteroidsGameManager.RestartGame += ResetGame;
	}
	void ResetGame()
	{
		_time = 0;
		CalculateNextSpanTime();

		foreach (EnemyMovement enemy in FindObjectsOfType<EnemyMovement>())
			Destroy(enemy.gameObject);
	}

	void Update()
	{
		if (_paused) return;

		_time += Time.deltaTime;

		if (_time >= _nextEnemySpawnTime)
		{
			SpawnEnemy(); 
			CalculateNextSpanTime();
		}

		if (Application.isEditor)
		{ 
			if (Input.GetKeyDown(spawnSmallEnemyDebugCode))
				Spawn(enemyPrefabsSmall);
			if (Input.GetKeyDown(spawnBigEnemyDebugCode))
				Spawn(enemyPrefabsBig);
		}
	}

	void CalculateNextSpanTime()
	{
		float t1 = nextEnemySpawnDurationOverTime1.Evaluate(_time);
		float t2 = nextEnemySpawnDurationOverTime2.Evaluate(_time);
		_nextEnemySpawnTime = _time + Random.Range(t1, t2);
	}

	void SpawnEnemy()
	{
		float p = probabilityOfBigEnemyOverTime.Evaluate(_time);
		GameObject enemyPrefab = Random.value < p ? enemyPrefabsBig : enemyPrefabsSmall;

		Spawn(enemyPrefab);
	}

	void Spawn(GameObject enemyPrefab)
	{
		Vector2 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
		GameObject newGO = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

		float healthMultiplier = healthMultiplierOverTime.Evaluate(_time);
		ImpactReceiver impactReceiver = newGO.GetComponent<ImpactReceiver>();
		impactReceiver.MaxHealth *= healthMultiplier;
		impactReceiver.CurrentHealth = impactReceiver.MaxHealth;

	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		foreach (Vector2 spawnPoint in spawnPoints)
		{
			Gizmos.DrawSphere(spawnPoint, 0.3f);
		}
	}
}