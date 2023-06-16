
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/Upgrade/Gun", fileName = "Upgrade Gun")]
class Upgrade_Gun : UpgradeSetup
{
	[Space]
	[SerializeField, Min(0)] float damageAmount = 0;
	[SerializeField, Min(0)] float damageMultiplier = 0;
	[SerializeField, Min(0)] float lifetimeAmount = 0;
	[SerializeField, Min(0)] float lifetimeMultiplier = 0;
	[SerializeField, Min(0)] float speedMultiplier = 0;
	[SerializeField, Min(0)] float gunFrequencyMultiplier = 0;
	[SerializeField, Min(0)] float magnetismAmount = 0;
	[SerializeField, Min(0)] float bulletPerShotAmount = 0;


	public override void ApplyUpgrade()
	{
		if (damageAmount != 0)
			Player.Gun.Damage += damageAmount; 
		if (damageMultiplier != 0)
			Player.Gun.Damage *= 1 + damageMultiplier;
		if (lifetimeAmount != 0)
			Player.Gun.Duration += lifetimeAmount;
		if (lifetimeMultiplier != 0)
			Player.Gun.Duration *= 1 + lifetimeMultiplier;
		if (speedMultiplier != 0)
			Player.Gun.TargetSpeed *= 1 + speedMultiplier;
		if (gunFrequencyMultiplier != 0)
		{ 
			float frequency = 1 / Player.Gun.Cooldown; 
			frequency *= 1 + gunFrequencyMultiplier; 
			Player.Gun.Cooldown = 1 / frequency; 
		}
		if (bulletPerShotAmount != 0)
			Player.Gun.BulletPerShot += bulletPerShotAmount;
		if (magnetismAmount != 0)
			Player.Gun.Magnetism += magnetismAmount;
	}

	public override string GetDescription()
	{ 
		if (damageAmount != 0)
			return $"Increase bullet's damage by {damageAmount}";
		if (damageMultiplier != 0)
			return $"Increase bullet's damage by {damageMultiplier * 100:0}%";
		if (lifetimeAmount != 0)
			return $"Increase bullet's lifetime by {lifetimeAmount} seconds";
		if (lifetimeMultiplier != 0)
			return $"Increase bullet's lifetime by {lifetimeMultiplier * 100:0}%";
		if (speedMultiplier != 0)
			return $"Increase bullet's speed by {speedMultiplier * 100:0}%";
		if (gunFrequencyMultiplier != 0)
			return $"Increase the gun's shooting frequency by {gunFrequencyMultiplier * 100:0}%";
		if (bulletPerShotAmount != 0)
			return $"Increase the bullet count per every shot by {bulletPerShotAmount}";
		if(magnetismAmount != 0)
			return $"Increase the bullet's magnetism";

		return "Upgrade gun...";
	}
}