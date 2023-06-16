
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/Upgrade/ShipMovement", fileName = "Upgrade ShipMovement")]
class Upgrade_ShipMovement : UpgradeSetup
{
	[Space]
	[SerializeField, Min(0)] float dragMultiplier = 0;
	[SerializeField, Min(0)] float accelerationMultiplier = 0;
	[SerializeField, Min(0)] float angularSpeedMultiplier = 0;

	public override void ApplyUpgrade()
	{
		if (dragMultiplier != 0)
			Player.Movement.Drag *= 1 + dragMultiplier;
		if (accelerationMultiplier != 0)
			Player.Movement.AccelerationMultiplier *= 1 + accelerationMultiplier;
		if (angularSpeedMultiplier != 0)
			Player.Movement.AngularSpeedMultiplier *= 1 + angularSpeedMultiplier;
	}

	public override string GetDescription()
	{ 
		string message = "Ship's ";
		if (accelerationMultiplier != 0)
			message += $"Acceleration is increased by: {accelerationMultiplier *100:+#;-#;0}%\n";
		if (dragMultiplier != 0)
			message += $"Drag is increased by: {dragMultiplier *100:+#;-#;0}%\n";
		if (angularSpeedMultiplier != 0)
			message += $"Angular Speed is increased by: {angularSpeedMultiplier *100:+#;-#;0}%\n";
		 
		return message;
	}
}