
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/Upgrade/StageSize", fileName = "Upgrade StageSize")]
class Upgrade_StageSize : UpgradeSetup
{
	[Space]
	[SerializeField, Min(0)] float screenSizeMultiplier = 0.1f; 

	public override void ApplyUpgrade()
	{
		if (screenSizeMultiplier != 0)
			CameraManager.TargetSize *= 1 + screenSizeMultiplier;
	}

	public override string GetDescription()
	{ 
		return $"Increase the size of the stage by {screenSizeMultiplier * 100}%";
	}
}