
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/Upgrade/Upgrade", fileName = "Upgrade Upgrade")]
class Upgrade_ExtraUpgrade : UpgradeSetup
{
	[Space]
	[SerializeField, Min(0)] int extraUpgradeAmount = 0; 

	public override void ApplyUpgrade()
	{
		if (extraUpgradeAmount != 0)
			UpgradeManager.UpgradeCount++;
	}

	public override string GetDescription()
	{ 
		return $"+{extraUpgradeAmount} extra upgrade to choose from";
	}
}