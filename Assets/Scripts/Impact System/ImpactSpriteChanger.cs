using UnityEngine;

public class ImpactSpriteChanger : MonoBehaviour, IImpactHandler
{
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Sprite[] sprites;

	void OnValidate()
	{
		if (spriteRenderer == null)
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	void Start()
	{
		UpdateSprite(1);
	}

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push)
	{
		UpdateSprite(receiver.HealthRate);
	}

	private void UpdateSprite(float healthRate)
	{
		int index = (int)(sprites.Length * (1 - healthRate));
		index = Mathf.Clamp(index, 0, sprites.Length - 1);
		spriteRenderer.sprite = sprites[index];
	}

	public void OnLostAllHealth(ImpactReceiver receiver) { }
}
