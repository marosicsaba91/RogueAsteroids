using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

class UpgradeManager : SingletonMonoBehaviour<UpgradeManager>
{
	[SerializeField] int upgradesToSpawn;
	[SerializeField] List<UpgradeSetup> allUpgrades;
	[SerializeField] List<Vector3> spawnPoints;
	[SerializeField] float startDelay;
	[SerializeField] float spawnDuration;

	readonly List<UpgradeSetup> _notUsedUpgrades = new();
	readonly List<UpgradeSetup> _usedUpgrades = new();
	readonly List<UpgradeSetup> _currentlyUsedUpgrades = new();

	int _upgradeCount;
	public static int UpgradeCount
	{
		get => Instance._upgradeCount;
		set => Instance._upgradeCount = value;
	}

	bool _enableSpawn;

	public static void EnableSpawn(bool enabled) => Instance._enableSpawn = enabled;

	protected sealed override void SingletonAwake()
	{
		AsteroidManager.AsteroidsCleared += SpawnUpgrades;
		AsteroidsGameManager.RestartGame += RestartGame;
		AsteroidsGameManager.NewLevelLoaded += (_) => _enableSpawn = true;
		Player.SpaceshipDied += () => _enableSpawn = false;

	}

	private void RestartGame()
	{
		_usedUpgrades.Clear();
		_notUsedUpgrades.Clear();
		_upgradeCount = upgradesToSpawn;
		foreach (UpgradeSetup upgradeSetup in allUpgrades)
		{
			for (int i = 0; i < upgradeSetup.count; i++)
				_notUsedUpgrades.Add(upgradeSetup);
		}
	}

	void SpawnUpgrades()
	{
		if (_enableSpawn)
			StartCoroutine(SpawnUpgradesRoutine());
	}

	IEnumerator SpawnUpgradesRoutine()
	{
		Debug.Log("SpawnUpgrades");
		_currentlyUsedUpgrades.Clear();

		yield return new WaitForSeconds(startDelay);
		for (int i = 0; i < UpgradeCount; i++)
		{
			Vector3 point = spawnPoints[i];
			int index = Random.Range(0, _notUsedUpgrades.Count);
			UpgradeSetup upgradeSetup = _notUsedUpgrades[index];
			if (_currentlyUsedUpgrades.Contains(upgradeSetup))
			{
				i--;
				continue;
			}
			yield return new WaitForSeconds(spawnDuration);
			_currentlyUsedUpgrades.Add(upgradeSetup);
			_usedUpgrades.Add(upgradeSetup);
			if (upgradeSetup.removeOnUse)
				_notUsedUpgrades.RemoveAt(index);

			Upgrade upgrade = Instantiate(upgradeSetup.prefab, point, Quaternion.identity);
			upgrade.Setup(upgradeSetup);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		foreach (Vector3 point in spawnPoints)
		{
			Gizmos.DrawSphere(point, 0.25f);
		}
	}

	static Coroutine _clearUpgradesCoroutine;

	internal static void Upgrade(UpgradeSetup upgradeSetup)
	{
		upgradeSetup.ApplyUpgrade();
		if (_clearUpgradesCoroutine != null)
			return;
		_clearUpgradesCoroutine = Instance.StartCoroutine(Instance.ClearUpgrades());
	}

	IEnumerator ClearUpgrades()
	{
		yield return null;

		Upgrade[] upgrades = FindObjectsOfType<Upgrade>();
		foreach (Upgrade upgrade in upgrades)
			upgrade.Lock();

		foreach (Upgrade upgrade in upgrades)
		{
			if (upgrade == null) continue;
			yield return new WaitForSeconds(spawnDuration);
			upgrade.Disappear();
			upgrade.Disappear();
		}

		Instance._usedUpgrades.Clear();
		AsteroidsGameManager.NextLevel();
		_clearUpgradesCoroutine = null;
	}
}