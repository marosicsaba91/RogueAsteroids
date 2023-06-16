using TMPro;
using UnityEngine;

public class Upgrade : MonoBehaviour, IImpactHandler
{
	[SerializeField] TMP_Text descriptionText;
	[SerializeField] SpriteRenderer upgradeImage;
	[SerializeField] ImpactReceiver impactReceiver;
	[SerializeField] Animator upgradeAnimator;

	UpgradeSetup upgradeSetup;

	private void OnValidate()
	{
		if (descriptionText == null)
			descriptionText = GetComponentInChildren<TMP_Text>();
		if (upgradeImage == null)
			upgradeImage = GetComponentInChildren<SpriteRenderer>();
		if (impactReceiver == null)
			impactReceiver = GetComponent<ImpactReceiver>();
		if (upgradeAnimator == null)
			upgradeAnimator = GetComponent<Animator>();
	}

	internal void Setup(UpgradeSetup upgradeSetup)
	{
		this.upgradeSetup = upgradeSetup;
		descriptionText.text = upgradeSetup.GetDescription();
		upgradeImage.sprite = upgradeSetup.icon;

	}

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push) { }

	public void OnLostAllHealth(ImpactReceiver receiver)
	{
		UpgradeManager.Upgrade(upgradeSetup);
	}

	internal void Disappear() => upgradeAnimator.SetBool("IsVisible", false);
	internal void Lock() => impactReceiver.EnableImpact = false;

	public void OnDisappear()
	{
		Destroy(gameObject);
	}
}