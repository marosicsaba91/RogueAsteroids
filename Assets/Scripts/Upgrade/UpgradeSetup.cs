
using UnityEngine;

abstract class UpgradeSetup : ScriptableObject
{
	public Upgrade prefab;
	public Sprite icon;
	public bool removeOnUse;
	public int count = 1;

	public abstract void ApplyUpgrade();
	public abstract string GetDescription();
}
