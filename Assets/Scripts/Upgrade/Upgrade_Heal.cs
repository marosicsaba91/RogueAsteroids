
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/Upgrade/ Heal", fileName = "Upgrade Heal")]
class Upgrade_Heal : UpgradeSetup
{
	[Space]
	[SerializeField, Min(0)] float healAmount = 0;
	[SerializeField, Min(0)] float healMultiplier = 0;

	[SerializeField, Min(0)] float raiseMaxHealthAmount = 0;
	[SerializeField, Min(0)] float raiseMaxHealthMultiplier = 0;

	public override void ApplyUpgrade()
	{
		if (healAmount != 0)
			Player.CurrentHealth += healAmount;
		if (healMultiplier != 0)
			Player.CurrentHealth += Player.MaxHealth * healMultiplier;

		if (raiseMaxHealthAmount != 0)
			Player.MaxHealth += raiseMaxHealthAmount;
		if (raiseMaxHealthMultiplier != 0)
			Player.MaxHealth += Player.MaxHealth * raiseMaxHealthMultiplier;
	}

	public override string GetDescription()
	{
		if (healAmount != 0)
			return $"Heal {healAmount} HP";
		if (healMultiplier != 0)
			return $"Heal {healMultiplier * 100:0}% of your max HP";
		if (raiseMaxHealthAmount != 0)
			return $"Increase max HP by {raiseMaxHealthAmount}";
		if (raiseMaxHealthMultiplier != 0)
			return $"Increase max HP by {raiseMaxHealthMultiplier * 100:0}%";

		return "Heal...";
	}
}